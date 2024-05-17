using System.Text.Json;
using System.Text.Json.Serialization;

namespace CEP.Api;

public record CepDto
{
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("estado")] public string State { get; set; } = string.Empty;

    [JsonPropertyName("cidade")] public string City { get; set; } = string.Empty;

    [JsonPropertyName("bairro")] public string Neighborhood { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")] public string Street { get; set; } = string.Empty;

    [JsonPropertyName("api_provedora")] public string ApiProvider { get; set; } = string.Empty;

    public override string ToString()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        return json;
    }
}