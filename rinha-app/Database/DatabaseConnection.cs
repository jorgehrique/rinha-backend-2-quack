using MongoDB.Driver;

public class DatabaseConnection : IDatabaseConnection
{
    private readonly IMongoClient _mongoClient;

    public DatabaseConnection()
    {
        // nao se faz isso em projeto real kk
        var settings = MongoClientSettings.FromConnectionString("mongodb://user:pass@mongo:27017");        
        settings.WriteConcern = WriteConcern.Unacknowledged; // prerigo!
        _mongoClient = new MongoClient(settings);
    }

    public Task<TransacaoSaldo> ExecutarTransacao(int id, TransacaoBody transacao)
    {
        var filterId = Builders<Cliente>.Filter.Eq("_id", id);
        long count = _mongoClient.GetDatabase("database")
            .GetCollection<Cliente>("clientes")
            .CountDocuments(filterId);

        if (count < 0)
        {
            throw new ClienteNotFoundException("Cliente não encontrado: Id=" + id);
        }

        var updateOptions = new FindOneAndUpdateOptions<Cliente> { ReturnDocument = ReturnDocument.After };
        Cliente? updated;

        Transacao t = new Transacao(transacao);

        var sortUltimasTransacoes = Builders<Transacao>.Sort.Descending("realizada_em");

        // pulo do gato
        var updateTransacoes = Builders<Cliente>.Update
            .PushEach(c => c.Ultimas_transacoes, new List<Transacao>() { t }, 10, 1, sortUltimasTransacoes);

        if (transacao.tipo.Equals('c'))
        {
            var updateSaldo = Builders<Cliente>.Update.Inc(c => c.Saldo, transacao.valor);
            var updates = Builders<Cliente>.Update.Combine(updateSaldo, updateTransacoes);
            updated = _mongoClient.GetDatabase("database")
                .GetCollection<Cliente>("clientes")
                .FindOneAndUpdate(filterId, updates, updateOptions);
        }
        else
        {
            var filterDebito = filterId & Builders<Cliente>.Filter.Where(c => c.Saldo + c.Limite >= transacao.valor);
            var updateSaldo = Builders<Cliente>.Update.Inc(c => c.Saldo, -transacao.valor);
            var updates = Builders<Cliente>.Update.Combine(updateSaldo, updateTransacoes);

            updated = _mongoClient.GetDatabase("database")
                .GetCollection<Cliente>("clientes")
                .FindOneAndUpdate(filterDebito, updates, updateOptions);
        }

        if (updated != null)
        {
            return Task.FromResult(new TransacaoSaldo(updated.Saldo, updated.Limite));
        }
        else
        {
            throw new TransacaoInvalidaException("Saldo Insuficiente: Id=" + id);
        }
    }

    public async Task<Extrato> GetExtrato(int id)
    {
        var filterId = Builders<Cliente>.Filter.Eq("_id", id);

        var cliente = _mongoClient.GetDatabase("database")
            .GetCollection<Cliente>("clientes")
            .Find(filterId)
            .FirstOrDefault()
            ?? throw new ClienteNotFoundException("Cliente não encontrado: Id=" + id);

        return await Task.FromResult(
            new Extrato(
                new Saldo(
                    cliente.Saldo,
                    DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"),
                    cliente.Limite
                ),
                cliente.Ultimas_transacoes
            )
        );
    }

}