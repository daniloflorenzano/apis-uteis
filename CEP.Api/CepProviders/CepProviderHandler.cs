using System.Text.Json;

namespace CEP.Api.CepProviders;

public class CepProviderHandler<T> : ICepProviderHandler where T : CepProviderBase
{
    private string ProviderName { get; } = typeof(T).Name;
    private JsonSerializerOptions JsonSerializerOptions { get; } = new() { PropertyNameCaseInsensitive = true };
    private string BaseUrl { get; } = Activator.CreateInstance<T>().BaseUrl;

    public async Task<CepDto> GetCepDtoAsync(string cep, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        if (cancellationToken == default)
            client.Timeout = TimeSpan.FromSeconds(10);
        
        var url = string.Format(BaseUrl, cep);
        var response = await client.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new CepNotFoundException($"Falha ao buscar CEP '{cep}' no provedor '{ProviderName}'. Status code: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var providerResponse = JsonSerializer.Deserialize<T>(content, JsonSerializerOptions) ??
                               throw new JsonException($"Falha ao deserializar CEP '{cep}' no provedor {ProviderName}.");
        
        var dto = providerResponse.MapToDto();
        
        // necessário esse tratamento pois engraçadinhos como o ViaCEP retornam 'status 200: erro true' quando não encontram o CEP
        // afinal, o que é um erro se não um sucesso em encontrar um erro? ¯\_(ツ)_/¯ 
        if (string.IsNullOrEmpty(dto.City))
            throw new CepNotFoundException($"CEP '{cep}' não encontrado no provedor '{ProviderName}'.");
        
        return dto;
    }
}