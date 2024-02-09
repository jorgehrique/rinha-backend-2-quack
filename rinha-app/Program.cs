var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/clientes/{id}/transacoes", (int id, Transacao transacao) => {

    Console.WriteLine("Id: " + id);
    Console.WriteLine("Transacao: " + transacao);

    if(!isTransacaoValid(transacao)){
        return Results.BadRequest();
    }

    return Results.Ok(new TransacaoExecutada(100000, -9098));
});

app.MapGet("/clientes/{id}/extrato", (int id) => {

    Console.WriteLine("Id: " + id);

    return Results.Ok(new Extrato(
        new Saldo(-9098, "2024-01-17T02:34:41.217753Z", 100000),
        new List<Transacao>{
            new Transacao(10, 'c', "descricao", "2024-01-17T02:34:38.543030Z"),
            new Transacao(90000, 'd', "descricao", "2024-01-17T02:34:38.543030Z")
        }
    ));
});

app.Run();

bool isTransacaoValid(Transacao transacao){
    return transacao.valor > 0
        && transacao.descricao.Length >= 1 && transacao.descricao.Length <= 10
        && (transacao.tipo != 'c' ||  transacao.tipo != 'd');
}

record Transacao(int valor, char tipo, string descricao, string? realizada_em)
{

}

record TransacaoExecutada(int limite, int saldo)
{

}

record Extrato(Saldo saldo, List<Transacao> ultimas_transacoes)
{

}

record Saldo(int total, string data_extrato, int limite){

}