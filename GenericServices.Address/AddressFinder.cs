using GenericServices.Address.Exceptions;
using GenericServices.Address.External;

namespace GenericServices.Address;

public class AddressFinder
{
    private readonly CepProvidersList _cepProvidersList;

    public AddressFinder(CepProvidersList cepProvidersList)
    {
        _cepProvidersList = cepProvidersList;
    }

    /// <summary>
    /// Busca em todos os provedores de CEP de forma paralela e retorna o primeiro resultado encontrado.
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    /// <exception cref="CepNotValidException"></exception>
    /// <exception cref="AddressNotFoundException"></exception>
    /// <exception cref="MissingCepProvidersException"></exception>
    public async Task<Address> FindByCepParallel(string cep)
    {
        if (_cepProvidersList.Count == 0)
            throw new MissingCepProvidersException();
        
        if (!CEP.IsValid(cep))
            throw new CepNotValidException($"'{cep}' não é um CEP válido.");
        
        var cancellationTokenSource = new CancellationTokenSource();
        var taskList = _cepProvidersList.Select(x => x.Request(cep, cancellationTokenSource.Token)).ToList();
        
        var providerResponse = await RunTasks(taskList, cancellationTokenSource);

        return ReturnAddressIfValid(providerResponse, cep);
    }

    // Referências:
    // https://stackoverflow.com/questions/41426418/task-whenany-what-happens-with-remaining-running-tasks
    // https://books.google.com.br/books?id=nsaUAwAAQBAJ&pg=PA26&lpg=PA26#v=onepage&q&f=false
    private static async Task<ICepProviderApiResponse?> RunTasks<T>(List<Task<T>> taskList, CancellationTokenSource cancellationTokenSource) where T : ICepProviderApiResponse?
    {
        var timeout = TimeSpan.FromSeconds(15);
        T dummyCancelledResult = default!;

        var timeoutOrCancellationTask = Task
            .Delay(timeout, cancellationTokenSource.Token)
            .ContinueWith(t => dummyCancelledResult, TaskContinuationOptions.ExecuteSynchronously);
        
        try
        {
            do
            {
                // Adiciona a task de timeout e cancelamento na lista de ainda pendentes
                var tasksToWaitAndTimeout = taskList.Union(new[] { timeoutOrCancellationTask}).ToList();

                // Aguarda a primeira task terminar
                var finishedTask = await Task.WhenAny(tasksToWaitAndTimeout).ConfigureAwait(false);

                // Caso a task que terminou tenha sido a de timeout
                if (finishedTask == timeoutOrCancellationTask)
                    return null;

                // Caso a task tenha terminado com sucesso e com um dos resultados esperados
                if (finishedTask.Status == TaskStatus.RanToCompletion)
                {
                    var result = await finishedTask;

                    if (result is not null)
                    {
                        // Cancelar as tarefas restantes
                        await cancellationTokenSource.CancelAsync();
                        
                        return result;
                    }
                }

                // Caso contrário, remove a task que terminou e continua aguardando as outras
                taskList.Remove(finishedTask);
            } while (!cancellationTokenSource.Token.IsCancellationRequested && taskList.Count != 0);

            // Se todas as tarefas terminaram e nenhuma retornou um resultado válido
            return null;
        }
        catch (Exception)
        {
            // Pode adicionar algum log ou manipulação de exceção aqui, se necessário
        }
        finally
        {
            // Cancelar as tarefas restantes se não foi feito anteriormente
            await cancellationTokenSource.CancelAsync();
        }

        // Caso tenha ocorrido cancelamento ou timeout
        return null;
    }
    
    private static Address ReturnAddressIfValid(ICepProviderApiResponse? providerResponse, string cep)
    {
        var address = providerResponse?.MapToDto();
        
        if (address?.City is null)
            throw new AddressNotFoundException($"Endereço não encontrado para o CEP '{cep}'");
        
        return address;
    }

    /// <summary>
    /// Busca em todos os provedores de CEP de forma sequencial e retorna o primeiro resultado encontrado.
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    /// <exception cref="CepNotValidException"></exception>
    /// <exception cref="AddressNotFoundException"></exception>
    /// <exception cref="MissingCepProvidersException"></exception>
    public async Task<Address> FindByCepSequential(string cep)
    {
        if (!CEP.IsValid(cep))
            throw new CepNotValidException($"'{cep}' não é um CEP válido.");
        
        if (_cepProvidersList.Count == 0)
            throw new MissingCepProvidersException();
        
        foreach (var provider in _cepProvidersList)
        {
            try
            {
                var providerResponse = await provider.Request(cep);
                if (providerResponse is null)
                    continue;
                
                return providerResponse.MapToDto();
            }
            catch (Exception)
            {
                // nao faz nada, tenta o proximo provedor
            }
        }

        throw new AddressNotFoundException($"Endereço não encontrado para o CEP '{cep}'");
    }
}