
interface IDatabaseConnection
{
    Task<TransacaoSaldo> ExecutarTransacao(int id, TransacaoBody transacao);
    Task<Extrato> GetExtrato(int id);
}