using MongoDB.Driver;

interface IDatabaseConnection
{
    IAsyncCursor<Cliente>? GetAllClientes();
    TransacaoSaldo ExecutarTransacao(int id, Transacao transacao);
}