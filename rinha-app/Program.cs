var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();

var app = builder.Build();

app.MapPost("/clientes/{id}/transacoes", async (int id, TransacaoBody transacao, IDatabaseConnection databaseConnection) =>
{
    if (!IsTransacaoValid(transacao))
    {
        return Results.UnprocessableEntity();
    }

    try
    {
        var saldo = await databaseConnection.ExecutarTransacao(id, transacao);
        return Results.Ok(saldo);
    }
    catch (ClienteNotFoundException exception)
    {
        return Results.NotFound(exception.Message);
    }
    catch (TransacaoInvalidaException exception)
    {
        return Results.UnprocessableEntity(exception.Message);
    }
});

app.MapGet("/clientes/{id}/extrato", async (int id, IDatabaseConnection databaseConnection) =>
{
    try
    {
        var extrato = await databaseConnection.GetExtrato(id);
        return Results.Ok(extrato);
    }
    catch (ClienteNotFoundException exception)
    {
        return Results.NotFound(exception.Message);
    }
});

app.Run();

static bool IsTransacaoValid(TransacaoBody transacao)
{
    return transacao.valor != null && transacao.valor > 0
        && transacao.descricao != null && transacao.descricao.Length >= 1 && transacao.descricao.Length <= 10
        && transacao.tipo != null && (transacao.tipo == 'c' || transacao.tipo == 'd');
}