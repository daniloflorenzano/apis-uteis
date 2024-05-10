using System.Text.Json.Serialization;

namespace CEP.Api;

public abstract class CepResponseBase
{
    [JsonIgnore]
    public abstract string BaseUrl { get; } 
    public abstract CepDto MapToDto();
}