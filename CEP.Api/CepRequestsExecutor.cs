namespace CEP.Api;

public class CepRequestsExecutor
{
    public Task<CepDto> ReturnFirstEndendResultFromAllProviders(string cep)
    {
        // Cria uma lista de tarefas taskLists do tipo Task<CepDto> com todas as chamadas de GetCepDtoAsync baseadas
        // nas classes de CepProvidersResponses de forma dinamica
        var taskList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.BaseType == typeof(CepResponseBase))
            .Select(x =>
                (ICepProviderHandler)Activator.CreateInstance(typeof(CepProviderHandler<>).MakeGenericType(x))!)
            .Select(x => x.GetCepDtoAsync(cep))
            .ToList();

        // Retorna a primeira tarefa que for concluida
        return Task.WhenAny(taskList).Unwrap();
    }
}