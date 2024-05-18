namespace GenericServices.Address.External.ProvidersDTOs;

public class BrasilApi : ICepProviderApiResponse
{
    public string Cep { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    
    
    public string BaseUrl { get; } = "https://brasilapi.com.br/api/cep/v1/{0}";
    
    public Address MapToDto()
    {
        return new Address
        {
            Cep = Cep,
            State = State,
            City = City,
            Neighborhood = Neighborhood,
            Street = Street,
            ApiProvider = nameof(BrasilApi)
        };
    }

    public bool FieldsAreValid()
    {
        return !string.IsNullOrWhiteSpace(Cep) && !string.IsNullOrWhiteSpace(State) &&
               !string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Neighborhood) &&
               !string.IsNullOrWhiteSpace(Street);
    }
}