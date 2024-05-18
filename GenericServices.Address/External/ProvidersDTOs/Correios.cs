// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GenericServices.Address.External.ProvidersDTOs;

public class Correios : ICepProviderApiResponse
{
    public string Cep { get; set; }
    public string Bairro { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public string End { get; set; }
    public string Complemento { get; set; }
    
    public string BaseUrl { get; } = string.Empty;

    public bool FieldsAreValid()
    {
        return !string.IsNullOrWhiteSpace(Cep) && !string.IsNullOrWhiteSpace(Bairro) &&
               !string.IsNullOrWhiteSpace(Uf) && !string.IsNullOrWhiteSpace(Cidade) &&
               !string.IsNullOrWhiteSpace(End);
    }

    public Address MapToDto()
    {
        return new Address
        {
            Cep = Cep,
            State = Uf,
            City = Cidade,
            Neighborhood = Bairro,
            Street = End,
            ApiProvider = nameof(Correios)
        };
    }
}