using System.Text.RegularExpressions;
using GenericServices.Address.Exceptions;

namespace GenericServices.Address;

// ReSharper disable once InconsistentNaming
public static class CEP
{
    public static bool IsValid(string cep)
    {
        // remove caracteres não numéricos
        cep = Regex.Replace(cep, "[^0-9]", "");
        
        return cep.Length == 8;
    }
    
    /// <summary>
    /// Adiciona a máscara de CEP ao valor informado.
    /// </summary>
    /// <param name="cep"></param>
    /// <returns>CEP no formato xxxxx-xxx</returns>
    /// <exception cref="CepNotValidException"></exception>
    public static string Format(string cep)
    {
        if (IsValid(cep))
            return $"{cep.Substring(0, 5)}-{cep.Substring(5, 3)}";

        throw new CepNotValidException($"'{cep}' não é um CEP válido.");
    }

    /// <summary>
    /// Remove a máscara de CEP do valor informado.
    /// </summary>
    /// <param name="cep"></param>
    /// <returns>CEP com apenas números</returns>
    /// <exception cref="CepNotValidException"></exception>
    public static string Unformat(string cep)
    {
        if (IsValid(cep))
            return cep.Replace("-", "");
        
        throw new CepNotValidException($"'{cep}' não é um CEP válido.");
    }
}