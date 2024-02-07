var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/clientes/{id}/transacoes", async (int id, Transacao transacao) => {
    Console.WriteLine("Id: " + id);
    Console.WriteLine("Transacao: " + transacao);
});

app.Run();

record Transacao(int valor, string tipo, string descricao)
{

}
