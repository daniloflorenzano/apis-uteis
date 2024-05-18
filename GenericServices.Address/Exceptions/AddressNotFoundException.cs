namespace GenericServices.Address.Exceptions;

[Serializable]
public class AddressNotFoundException : Exception
{
    public AddressNotFoundException() : base()
    {
        if (!string.IsNullOrEmpty(base.Message))
            Console.WriteLine(base.Message);
        
        Console.WriteLine("Endereço não encontrado.");
    }
    public AddressNotFoundException(string message) : base(message) { }
    public AddressNotFoundException(string message, Exception inner) : base(message, inner) { }
}