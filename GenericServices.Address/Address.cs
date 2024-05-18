using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericServices.Address;

/// <summary>
/// DTO para representar um endereço.
/// </summary>
public record Address
{
    [JsonPropertyName("cep")] public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("cep_formatado")] public string FormattedCep => CEP.Format(Cep);

    [JsonPropertyName("estado")] public string State { get; set; } = string.Empty;

    [JsonPropertyName("cidade")] public string City { get; set; } = string.Empty;

    [JsonPropertyName("bairro")] public string Neighborhood { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")] public string Street { get; set; } = string.Empty;

    [JsonPropertyName("api_provedora")] public string ApiProvider { get; set; } = string.Empty;

    public override string ToString()
    {
        // O JavaScriptEncoder.UnsafeRelaxedJsonEscaping permite que caracteres não ASCII sejam mantidos em sua forma original em vez de serem escapados.
        var json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        
        return json;
    }
}