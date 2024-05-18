namespace CEP.Api.CepProviders;

public class ViaCep : ICepProviderApiResponse 
{
    public string cep { get; set; }
    public string logradouro { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string localidade { get; set; }
    public string uf { get; set; }
    public string ibge { get; set; }
    public string gia { get; set; }
    public string ddd { get; set; }
    public string siafi { get; set; }

    public string BaseUrl { get; } = "https://viacep.com.br/ws/{0}/json/";
    public Address MapToDto()
    {
        return new Address
        {
            Cep = cep,
            State = uf,
            City = localidade,
            Neighborhood = bairro,
            Street = logradouro,
            ApiProvider = nameof(ViaCep)
        };
    }

    public bool FieldsAreValid()
    {
        return !string.IsNullOrWhiteSpace(cep) && !string.IsNullOrWhiteSpace(logradouro) &&
               !string.IsNullOrWhiteSpace(bairro) && !string.IsNullOrWhiteSpace(localidade) &&
               !string.IsNullOrWhiteSpace(uf);
    }
}