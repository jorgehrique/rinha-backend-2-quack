using MongoDB.Driver;

public class DatabaseConnection : IDatabaseConnection
{
    private readonly IMongoCollection<Cliente> _clientesCollection;

    public DatabaseConnection()
    {
        // nao se faz isso em projeto real kk
        var settings = MongoClientSettings.FromConnectionString("mongodb://user:pass@mongo:27017"); 
        
        // settings.WriteConcern = WriteConcern.Unacknowledged;
        // settings.MaxConnectionPoolSize = 50;
        // settings.MinConnectionPoolSize = 25;

        _clientesCollection = new MongoClient(settings)
            .GetDatabase("database")
            .GetCollection<Cliente>("clientes");
    }

    public Task<TransacaoSaldo> ExecutarTransacao(int id, Transacao transacao)
    {
        var filterId = Builders<Cliente>.Filter.Eq("_id", id);
        long count = _clientesCollection.CountDocuments(filterId);

        if (count < 0)
        {
            throw new ClienteNotFoundException("Cliente não encontrado: Id=" + id);
        }

        var updateOptions = new FindOneAndUpdateOptions<Cliente> { ReturnDocument = ReturnDocument.After };
        Cliente? updated;

        if (transacao.tipo.Equals('c'))
        {
            var update = Builders<Cliente>.Update.Inc(c => c.Saldo, transacao.valor);
            updated = _clientesCollection.FindOneAndUpdate(filterId, update, updateOptions);
        }
        else
        {
            var filterDebito = filterId & Builders<Cliente>.Filter.Where(c => c.Saldo + c.Limite >= transacao.valor);
            var update = Builders<Cliente>.Update.Inc(c => c.Saldo, -transacao.valor);
            updated = _clientesCollection.FindOneAndUpdate(filterDebito, update, updateOptions);
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

    public Task<Extrato> GetExtrato(int id)
    {
        var filterId = Builders<Cliente>.Filter.Eq("_id", id);
        
        var cliente = _clientesCollection.Find(filterId).FirstOrDefault() 
            ?? throw new ClienteNotFoundException("Cliente não encontrado: Id=" + id);

        return Task.FromResult(
            new Extrato(
                new Saldo(
                    cliente.Saldo, 
                    DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK"),
                    cliente.Limite
                ),
                ["ultima transacao"]
        ));
    }

}