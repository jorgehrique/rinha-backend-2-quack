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

        if (transacao.tipo.Equals('c'))
        {
            var update = Builders<Cliente>.Update.Inc(c => c.Saldo, transacao.valor);
            updated = _mongoClient.GetDatabase("database")
                .GetCollection<Cliente>("clientes")
                .FindOneAndUpdate(filterId, update, updateOptions);
        }
        else
        {
            var filterDebito = filterId & Builders<Cliente>.Filter.Where(c => c.Saldo + c.Limite >= transacao.valor);
            var update = Builders<Cliente>.Update.Inc(c => c.Saldo, -transacao.valor);
            updated = _mongoClient.GetDatabase("database")
                .GetCollection<Cliente>("clientes")
                .FindOneAndUpdate(filterDebito, update, updateOptions);
        }

        if (updated != null)
        {
            SalvarTransacao(id, transacao);
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

        var filterTransacao = Builders<Transacao>.Filter.Eq("cliente_id", id);

        var ultimasTransacoes = await _mongoClient.GetDatabase("database")
            .GetCollection<Transacao>("transacoes")
            .Find(filterTransacao)
            .Limit(10)
            .SortByDescending(t => t.Realizada_em)
            .ToListAsync();

        return await Task.FromResult(
            new Extrato(
                new Saldo(
                    cliente.Saldo,
                    DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"),
                    cliente.Limite
                ),
                ultimasTransacoes.ToList()
            )
        );
    }

    private async void SalvarTransacao(int id, TransacaoBody body)
    {
        await Task.Run(() =>
        {
            Transacao t = new Transacao(
                id,
                body.valor,
                body.tipo,
                body.descricao
            );

            _mongoClient.GetDatabase("database")
                .GetCollection<Transacao>("transacoes")
                .InsertOne(t);
        });
    }

}