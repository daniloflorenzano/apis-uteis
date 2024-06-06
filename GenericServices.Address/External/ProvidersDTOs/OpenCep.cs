namespace GenericServices.Address.External.ProvidersDTOs;

public class OpenCep : ICepProviderApiResponse
{
    public string cep { get; set; }
    public string logradouro { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string localidade { get; set; }
    public string uf { get; set; }
    public string ibge { get; set; }
    
    public string BaseUrl { get; } = "https://opencep.com/v1/{0}.json";
    
    public bool FieldsAreValid()
    {
        return !string.IsNullOrWhiteSpace(cep) && !string.IsNullOrWhiteSpace(logradouro) &&
               !string.IsNullOrWhiteSpace(bairro) && !string.IsNullOrWhiteSpace(localidade) &&
               !string.IsNullOrWhiteSpace(uf);
    }

    public Address MapToDto()
    {
        return new Address
        {
            Cep = cep,
            State = uf,
            City = localidade,
            Neighborhood = bairro,
            Street = logradouro,
            ApiProvider = nameof(OpenCep)
        };
    }
}