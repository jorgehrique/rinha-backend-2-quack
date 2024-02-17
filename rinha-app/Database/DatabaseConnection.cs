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

}