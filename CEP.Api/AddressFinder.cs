using CEP.Api.CepProviders;
// ReSharper disable All

namespace CEP.Api;

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
    public async Task<Address?> FindByCepParallel(string cep)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var taskList = _cepProvidersList.Select(x => x.Request(cep, cancellationTokenSource.Token)).ToList();
        var providerResponse = await RunTasks(taskList, cancellationTokenSource);
        
        if (providerResponse is null)
            return null;
        
        var address = providerResponse.MapToDto();
        
        if (address.City is null)
            return null;
        
        return address;
    }

    // Referências:
    // https://stackoverflow.com/questions/41426418/task-whenany-what-happens-with-remaining-running-tasks
    // https://books.google.com.br/books?id=nsaUAwAAQBAJ&pg=PA26&lpg=PA26#v=onepage&q&f=false
    private async Task<ICepProviderApiResponse?> RunTasks<T>(List<Task<T>> taskList, CancellationTokenSource cancellationTokenSource) where T : ICepProviderApiResponse
    {
        var timeout = TimeSpan.FromSeconds(15);
        T dummyCancelledResult = default!;

        Task<T> timeoutOrCancellationTask = Task
            .Delay(timeout, cancellationTokenSource.Token)
            .ContinueWith(t => dummyCancelledResult, TaskContinuationOptions.ExecuteSynchronously);

        try
        {
            do
            {
                // Adiciona a task de timeout e cancelamento na lista de ainda pendentes
                var tasksToWaitAndTimeout = taskList.Union<Task<T>>(new[] { timeoutOrCancellationTask}!).ToList();

                // Aguarda a primeira task terminar
                var finishedTask = await Task.WhenAny(tasksToWaitAndTimeout).ConfigureAwait(false);

                // Caso a task que terminou tenha sido a de timeout
                if (finishedTask == timeoutOrCancellationTask)
                    return null;

                // Caso a task tenha terminado com sucesso e com um dos resultados esperados
                if (finishedTask.Status == TaskStatus.RanToCompletion)
                {
                    // Cancelar as tarefas restantes
                    await cancellationTokenSource.CancelAsync();
                    var result = await finishedTask as ICepProviderApiResponse;
                    return result;
                }

                // Caso contrário, remove a task que terminou e continua aguardando as outras
                taskList.Remove(finishedTask);
            } while (!cancellationTokenSource.Token.IsCancellationRequested && taskList.Count != 0);
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

    /// <summary>
    /// Busca em todos os provedores de CEP de forma sequencial e retorna o primeiro resultado encontrado.
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<Address?> FindByCepSequential(string cep)
    {
        foreach (var provider in _cepProvidersList)
        {
            try
            {
                var providerResponse = await provider.Request(cep);
                return providerResponse.MapToDto();
            }
            catch (Exception)
            {
                // nao faz nada, tenta o proximo provedor
            }
        }

        return null;
    }
}