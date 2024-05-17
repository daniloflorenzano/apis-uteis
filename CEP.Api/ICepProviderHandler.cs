namespace CEP.Api;

public interface ICepProviderHandler
{
    Task<CepDto> GetCepDtoAsync(string cep, CancellationToken cancellationToken = default);
}