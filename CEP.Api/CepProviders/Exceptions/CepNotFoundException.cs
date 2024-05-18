namespace CEP.Api.CepProviders.Exceptions;

[Serializable]
public class CepNotFoundException : Exception
{
    public CepNotFoundException() : base() { }
    public CepNotFoundException(string message) : base(message) { }
    public CepNotFoundException(string message, Exception inner) : base(message, inner) { }
}