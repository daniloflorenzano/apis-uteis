using System.Text.Json.Serialization;

namespace CEP.Api;

public record CepDto
{
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("Estado")] public string State { get; set; } = string.Empty;

    [JsonPropertyName("Cidade")] public string City { get; set; } = string.Empty;

    [JsonPropertyName("Bairro")] public string Neighborhood { get; set; } = string.Empty;

    [JsonPropertyName("Logradouro")] public string Street { get; set; } = string.Empty;
}