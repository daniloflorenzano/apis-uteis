using System.Text.Json;
using CEP.Api.CepProviders.Exceptions;

namespace CEP.Api.CepProviders;

public class CepProviderHandler<T> : ICepProviderHandler where T : ICepProvider
{
    public async Task<CepDto> GetCepDtoAsync(string cep, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        if (cancellationToken == default)
            client.Timeout = TimeSpan.FromSeconds(10);
        
        var baseUrl = Activator.CreateInstance<T>().BaseUrl;
        var url = string.Format(baseUrl, cep);
        var response = await client.GetAsync(url, cancellationToken);

        var providerName = typeof(T).Name;
        if (!response.IsSuccessStatusCode)
            throw new CepNotFoundException($"Falha ao buscar CEP '{cep}' no provedor '{providerName}'. Status code: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var providerResponse = JsonSerializer.Deserialize<T>(content) ??
                               throw new JsonException($"Falha ao deserializar CEP '{cep}' no provedor {providerName}.");
        
        var dto = providerResponse.MapToDto();
        
        // necessário esse tratamento pois engraçadinhos como o ViaCEP retornam 'status 200: erro true' quando não encontram o CEP
        // afinal, o que é um erro se não um sucesso em encontrar um erro? ¯\_(ツ)_/¯ 
        if (string.IsNullOrEmpty(dto.City))
            throw new CepNotFoundException($"CEP '{cep}' não encontrado no provedor '{providerName}'.");
        
        return dto;
    }
}