using GenericServices.Address;
using GenericServices.Address.Exceptions;
using GenericServices.Address.External;

namespace TestProject.Address;

public class AddressFinderTests
{
    private CepProvidersList _cepProvidersList;

    [SetUp]
    public void Setup()
    {
        _cepProvidersList = CepProvidersList.Instance;
    }
    
    [Test]
    public async Task FindCepParallel_Should_Work_Single_Valid_Cep()
    {
        var cepService = new AddressFinder(_cepProvidersList);
        var cepDto = await cepService.FindByCepParallel("27521620");

        TestContext.WriteLine(cepDto.ToString());
    }

    [Test]
    public async Task FindCepSequential_Should_Work_Single_Valid_Cep()
    {
        var cepService = new AddressFinder(_cepProvidersList);
        var cepDto = await cepService.FindByCepSequential("27521605");

        TestContext.WriteLine(cepDto.ToString());
    }

    [Test]
    public async Task FindCepParallel_Should_Work()
    {
        var cepService = new AddressFinder(_cepProvidersList);
        for (var i = 27521600; i < 27521620; i++)
        {
            try
            {
                var cepDto = await cepService.FindByCepParallel(i.ToString());

                TestContext.WriteLine(cepDto.ToString());
            }
            catch (Exception)
            {
                // nao faz nada
            }
        }
    }

    [Test]
    public async Task FindCepSequential_Should_Work()
    {
        var cepService = new AddressFinder(_cepProvidersList);
        for (var i = 27521600; i < 27521620; i++)
        {
            try
            {
                var cepDto = await cepService.FindByCepSequential(i.ToString());

                TestContext.WriteLine(cepDto.ToString());
            }
            catch (Exception)
            {
                // nao faz nada
            }
        }
    }

    [Test]
    public void FindCepParallel_Should_Throw_Exception_With_Invalid_Cep()
    {
        var cepService = new AddressFinder(_cepProvidersList);
        const string cep = "2752162a";

        Assert.ThrowsAsync<CepNotValidException>(async () => await cepService.FindByCepParallel(cep));
    }

    [Test]
    public void FindCepSequential_Should_Throw_Exception_With_Invalid_Cep()
    {
        var cepService = new AddressFinder(_cepProvidersList);
        const string cep = "2752162b";

        Assert.ThrowsAsync<CepNotValidException>(async () => await cepService.FindByCepSequential(cep));
    }
}