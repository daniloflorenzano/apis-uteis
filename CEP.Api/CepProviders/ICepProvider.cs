using System.Text.Json.Serialization;

namespace CEP.Api.CepProviders;

public interface ICepProvider
{
    [JsonIgnore]
    public string BaseUrl { get; } 
    public CepDto MapToDto();
}