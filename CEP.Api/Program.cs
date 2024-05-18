using CEP.Api;
using CEP.Api.CepProviders;

var builder = WebApplication.CreateSlimBuilder(args);

// Resgira como singleton para que a lista de provedores seja instanciada apenas uma vez
builder.Services.AddSingleton<CepProvidersList>();

var app = builder.Build();

app.MapGet("/cep/{cep}", async (CepProvidersList cepProvidersList, string cep) =>
{
    try
    {
        var executor = new AddressFinder(cepProvidersList);
        var cepDto = await executor.FindByCepParallel(cep);

        return Results.Ok(cepDto);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});


app.Run();
