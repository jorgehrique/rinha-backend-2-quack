using System.Text.Json.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, TransacaoContext.Default);    
    options.SerializerOptions.TypeInfoResolverChain.Insert(1, TransacaoExecutadaContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Insert(2, ExtratoContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Insert(3, SaldoExecutadaContext.Default);
});

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();

var app = builder.Build();

app.MapPost("/clientes/{id}/transacoes", (int id, Transacao transacao, IDatabaseConnection databaseConnection) => {

    Console.WriteLine("Id: " + id);
    Console.WriteLine("Transacao: " + transacao);

    if(!isTransacaoValid(transacao)){
        return Results.BadRequest();
    }

    databaseConnection.ExecutarTransacao(id, transacao);

    return Results.Ok(new TransacaoExecutada(100000, -9098));
});

app.MapGet("/clientes/{id}/extrato", (int id, IDatabaseConnection databaseConnection) => {

    Console.WriteLine("Id: " + id);

    // Teste database connection
    databaseConnection.GetAllClientes().ForEachAsync(c => {
        Console.WriteLine("Cliente: " + c.Limite);
    });

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

record TransacaoExecutada(int limite, int saldo);

record Extrato(Saldo saldo, List<Transacao> ultimas_transacoes);

record Saldo(int total, string data_extrato, int limite);

[JsonSerializable(typeof(Transacao))]
internal partial class TransacaoContext : JsonSerializerContext
{

}

[JsonSerializable(typeof(TransacaoExecutada))]
internal partial class TransacaoExecutadaContext : JsonSerializerContext
{

}

[JsonSerializable(typeof(Extrato))]
internal partial class ExtratoContext : JsonSerializerContext
{

}

[JsonSerializable(typeof(Saldo))]
internal partial class SaldoExecutadaContext : JsonSerializerContext
{

}