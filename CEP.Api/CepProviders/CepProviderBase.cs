using System.Text.Json.Serialization;

namespace CEP.Api.CepProviders;

public abstract class CepProviderBase
{
    [JsonIgnore]
    public abstract string BaseUrl { get; } 
    public abstract CepDto MapToDto();
}