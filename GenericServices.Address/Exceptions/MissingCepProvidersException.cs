namespace GenericServices.Address.Exceptions;

[Serializable]
public class MissingCepProvidersException : Exception
{
    public MissingCepProvidersException() : base()
    {
        if (!string.IsNullOrEmpty(base.Message))
            Console.WriteLine(base.Message);
        
        Console.WriteLine("Nenhum provedor de CEP foi encontrado.");
    }
    public MissingCepProvidersException(string message) : base(message) { }
    public MissingCepProvidersException(string message, Exception inner) : base(message, inner) { }
}