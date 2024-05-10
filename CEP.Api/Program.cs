using CEP.Api;

var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

app.MapGet("/cep/{cep}", async (string cep) =>
{
    try
    {
        // var providerHandler = new CepProviderHandler<BrasilApi>();
        // var cepDto = await providerHandler.GetCepDtoAsync(cep);
        
        var executor = new CepRequestsExecutor();
        var cepDto = await executor.ReturnFirstEndendResultFromAllProviders(cep);

        return Results.Ok(cepDto);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});


app.Run();
