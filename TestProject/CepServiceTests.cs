using CEP.Api;
using CEP.Api.CepProviders;

namespace TestProject;

public class CepServiceTests
{
    private CepProvidersList _cepProvidersList;
    
    [SetUp]
    public void Setup()
    {
        _cepProvidersList = new CepProvidersList();
    }

    [Test]
    public async Task FindCepParallel_Should_Work()
    {
        var cepService = new CepService(_cepProvidersList);
        for (int i = 27521600; i < 27521610; i++)
        {
            var cep = i.ToString();
            var cepDto = await cepService.FindCepParallel(cep);
            
            if (cepDto is null)
                TestContext.WriteLine($"CEP {i} não encontrado em nenhum provedor.");
            else
                TestContext.WriteLine(cepDto.ToString());
        }
    }
    
    [Test]
    public async Task FindCepSequential_Should_Work()
    {
        var cepService = new CepService(_cepProvidersList);
        for (int i = 27521600; i < 27521610; i++)
        {
            var cep = i.ToString();
            var cepDto = await cepService.FindCepSequential(cep);
            
            if (cepDto is null)
                TestContext.WriteLine($"CEP {i} não encontrado em nenhum provedor.");
            else
                TestContext.WriteLine(cepDto.ToString());
        }
    }
}