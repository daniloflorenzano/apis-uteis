namespace CEP.Api.CepProvidersResponses;

public class BrasilApi : CepResponseBase
{
    public string Cep { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    
    
    public override string BaseUrl { get; } = "https://brasilapi.com.br/api/cep/v1/{0}";
    
    public override CepDto MapToDto()
    {
        return new CepDto
        {
            Cep = Cep,
            State = State,
            City = City,
            Neighborhood = Neighborhood,
            Street = Street
        };
    }
}