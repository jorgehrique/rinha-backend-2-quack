public record Extrato(Saldo saldo, List<Transacao>? ultimas_transacoes);

public record Saldo(int total, string data_extrato, int limite);
