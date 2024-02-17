using MongoDB.Driver;

interface IDatabaseConnection
{
    IAsyncCursor<Cliente>? GetAllClientes();
}