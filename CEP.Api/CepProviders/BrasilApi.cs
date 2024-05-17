namespace CEP.Api.CepProviders;

public class BrasilApi : ICepProvider
{
    public string Cep { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    
    
    public string BaseUrl { get; } = "https://brasilapi.com.br/api/cep/v1/{0}";
    
    public CepDto MapToDto()
    {
        return new CepDto
        {
            Cep = Cep,
            State = State,
            City = City,
            Neighborhood = Neighborhood,
            Street = Street,
            ApiProvider = nameof(BrasilApi)
        };
    }
}