using System.Text.Json.Serialization;

namespace GenericServices.Address.External;

/// <summary>
/// DTO para representar o retorno da API de um provedor de CEP.
/// </summary>
public interface ICepProviderApiResponse
{
    [JsonIgnore]
    public string BaseUrl { get; } 
    public bool FieldsAreValid();
    
    /// <summary>
    /// Mapeia os campos da resposta da API para um DTO de endereço.
    /// </summary>
    /// <returns></returns>
    public Address MapToDto();
}