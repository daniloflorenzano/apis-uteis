using System.Collections;

namespace CEP.Api.CepProviders;

public class CepProvidersList : IList<ICepProviderApi>
{
    private readonly IList<ICepProviderApi> _listImplementation;

    public CepProvidersList()
    {
        _listImplementation = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(ICepProviderApiResponse).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .Select(x =>
                (ICepProviderApi)Activator.CreateInstance(typeof(CepProviderApi<>).MakeGenericType(x))!)
            .ToList();
    }

    # region IList Implementation

    public IEnumerator<ICepProviderApi> GetEnumerator()
    {
        return _listImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_listImplementation).GetEnumerator();
    }

    public void Add(ICepProviderApi item)
    {
        _listImplementation.Add(item);
    }

    public void Clear()
    {
        _listImplementation.Clear();
    }

    public bool Contains(ICepProviderApi item)
    {
        return _listImplementation.Contains(item);
    }

    public void CopyTo(ICepProviderApi[] array, int arrayIndex)
    {
        _listImplementation.CopyTo(array, arrayIndex);
    }

    public bool Remove(ICepProviderApi item)
    {
        return _listImplementation.Remove(item);
    }

    public int Count => _listImplementation.Count;

    public bool IsReadOnly => _listImplementation.IsReadOnly;

    public int IndexOf(ICepProviderApi item)
    {
        return _listImplementation.IndexOf(item);
    }

    public void Insert(int index, ICepProviderApi item)
    {
        _listImplementation.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _listImplementation.RemoveAt(index);
    }

    public ICepProviderApi this[int index]
    {
        get => _listImplementation[index];
        set => _listImplementation[index] = value;
    }

    # endregion
}