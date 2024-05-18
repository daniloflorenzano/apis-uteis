namespace GenericServices.Address.Exceptions;

[Serializable]
public class CepNotValidException : Exception
{
    public CepNotValidException() : base()
    {
        if (!string.IsNullOrEmpty(base.Message))
            Console.WriteLine(base.Message);
        
        Console.WriteLine("CEP inválido.");
    }
    public CepNotValidException(string message) : base(message) { }
    public CepNotValidException(string message, Exception inner) : base(message, inner) { }
}