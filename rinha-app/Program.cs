var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();

var app = builder.Build();

app.MapPost("/clientes/{id}/transacoes", (int id, Transacao transacao, IDatabaseConnection databaseConnection) =>
{
    if (!IsTransacaoValid(transacao))
    {
        return Results.BadRequest();
    }

    try
    {
        var saldo = databaseConnection.ExecutarTransacao(id, transacao);
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

app.MapGet("/clientes/{id}/extrato", (int id, IDatabaseConnection databaseConnection) =>
{
    try
    {
        var extrato = databaseConnection.GetExtrato(id);
        return Results.Ok(extrato);
    }
    catch (ClienteNotFoundException exception)
    {
        return Results.NotFound(exception.Message);
    }
});

app.Run();

static bool IsTransacaoValid(Transacao transacao)
{
    return transacao.valor > 0
        && transacao.descricao != null && transacao.descricao.Length >= 1 && transacao.descricao.Length <= 10
        && (transacao.tipo == 'c' || transacao.tipo == 'd');
}