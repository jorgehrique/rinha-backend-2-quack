using MongoDB.Driver;

public class DatabaseConnection : IDatabaseConnection
{
    private MongoClient? client = null;

    MongoClient GetClient()
    {
        client ??= new MongoClient("mongodb://user:pass@mongo:27017");
        return client;
    }

    public IAsyncCursor<Cliente>? GetAllClientes()
    {
        var _clientesCollection = GetClient().GetDatabase("database").GetCollection<Cliente>("clientes");
        var filter = Builders<Cliente>.Filter.Empty;
        return _clientesCollection.Find(filter).ToCursor();
    }

    public TransacaoSaldo ExecutarTransacao(int id, Transacao transacao)
    {
        var _clientesCollection = GetClient().GetDatabase("database").GetCollection<Cliente>("clientes");
        var filterId = Builders<Cliente>.Filter.Eq("_id", id);
        long count = _clientesCollection.CountDocuments(filterId);

        if (count < 0)
        {
            throw new ClienteNotFoundException("Cliente não encontrado: Id=" + id);
        }

        var updateOptions = new FindOneAndUpdateOptions<Cliente> { ReturnDocument = ReturnDocument.Before };
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
            return new TransacaoSaldo(updated.Saldo, updated.Limite);
        }
        else
        {
            throw new TransacaoInvalidaException("Saldo Insuficiente: Id=" + id);
        }
    }

}