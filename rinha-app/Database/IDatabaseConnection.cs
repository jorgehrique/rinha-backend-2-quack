using MongoDB.Driver;

interface IDatabaseConnection
{
    IAsyncCursor<Cliente>? GetAllClientes();
    bool ExecutarTransacao(int id, Transacao transacao);
}