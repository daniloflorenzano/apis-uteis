using System.Text.Json;

namespace GenericServices.Address.External;

public class CepProviderApi<T> : ICepProviderApi where T : ICepProviderApiResponse
{
    public async Task<ICepProviderApiResponse?> Request(string cep, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        if (cancellationToken == default)
            client.Timeout = TimeSpan.FromSeconds(10);
        
        var baseUrl = Activator.CreateInstance<T>().BaseUrl;
        var url = string.Format(baseUrl, cep);
        var response = await client.GetAsync(url, cancellationToken);

        var providerName = typeof(T).Name;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro ao buscar CEP {cep} no provedor {providerName}. Status: {response.StatusCode}");
            return null;
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        
        // necessário esse tratamento pois engraçadinhos como o ViaCEP retornam 'status 200: erro true' quando não encontram o CEP
        // afinal, o que é um erro se não um sucesso em encontrar um erro? ¯\_(ツ)_/¯ 
        if (result is null || !result.FieldsAreValid())
        {
            Console.WriteLine($"Erro ao buscar CEP {cep} no provedor {providerName}. Status: {response.StatusCode}");
            return null;
        }
        
        return result;
    }
}