
interface IDatabaseConnection
{
    TransacaoSaldo ExecutarTransacao(int id, Transacao transacao);
    Extrato GetExtrato(int id);
}