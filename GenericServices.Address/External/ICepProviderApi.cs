namespace GenericServices.Address.External;

public interface ICepProviderApi
{
    Task<ICepProviderApiResponse?> Request(string cep, CancellationToken cancellationToken = default);
}