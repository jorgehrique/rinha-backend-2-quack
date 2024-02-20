
interface IDatabaseConnection
{
    Task<TransacaoSaldo> ExecutarTransacao(int id, Transacao transacao);
    Task<Extrato> GetExtrato(int id);
}