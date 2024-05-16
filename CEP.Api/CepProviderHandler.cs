using System.Text.Json;
using Exception = System.Exception;

namespace CEP.Api;

public class CepProviderHandler<T> : ICepProviderHandler where T : CepResponseBase
{
    private string ProviderName { get; } = typeof(T).Name;
    private JsonSerializerOptions JsonSerializerOptions { get; } = new() { PropertyNameCaseInsensitive = true };
    private string BaseUrl { get; } = Activator.CreateInstance<T>().BaseUrl;

    public async Task<CepDto> GetCepDtoAsync(string cep)
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(0.1);
        
        var url = string.Format(BaseUrl, cep);
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Falha ao buscar CEP no provedor {ProviderName}. Status code: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();

        var providerResponse = JsonSerializer.Deserialize<T>(content, JsonSerializerOptions) ??
                               throw new Exception($"Falha ao deserializar CEP no provedor {ProviderName}.");

        Console.WriteLine($"CEP encontrado no provedor {ProviderName}.");
        return providerResponse.MapToDto();
    }
}