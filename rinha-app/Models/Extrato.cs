public record Extrato(Saldo saldo, string[]? ultimas_transacoes);

public record Saldo(int total, string data_extrato, int limite);
