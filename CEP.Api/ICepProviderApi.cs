using CEP.Api.CepProviders;

namespace CEP.Api;

public interface ICepProviderApi
{
    Task<ICepProviderApiResponse> Request(string cep, CancellationToken cancellationToken = default);
}