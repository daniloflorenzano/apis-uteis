namespace CEP.Api.CepProviders;

public class ViaCep : CepProviderBase 
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

    public override string BaseUrl { get; } = "https://viacep.com.br/ws/{0}/json/";
    public override CepDto MapToDto()
    {
        return new CepDto
        {
            Cep = cep,
            State = uf,
            City = localidade,
            Neighborhood = bairro,
            Street = logradouro,
            ApiProvider = nameof(ViaCep)
        };
    }
}