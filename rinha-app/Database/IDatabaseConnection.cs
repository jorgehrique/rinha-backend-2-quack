using MongoDB.Driver;

interface IDatabaseConnection
{
    IAsyncCursor<Cliente>? GetAllClientes();
    void ExecutarTransacao(int id, Transacao transacao);
}