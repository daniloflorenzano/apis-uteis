using System.Collections;

namespace CEP.Api.CepProviders;

public class CepProvidersList : IList<ICepProviderHandler>
{
    private IList<ICepProviderHandler> _listImplementation;

    public CepProvidersList()
    {
        _listImplementation = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.BaseType == typeof(CepProviderBase))
            .Select(x =>
                (ICepProviderHandler)Activator.CreateInstance(typeof(CepProviderHandler<>).MakeGenericType(x))!)
            .ToList();
    }

    # region IList Implementation

    public IEnumerator<ICepProviderHandler> GetEnumerator()
    {
        return _listImplementation.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_listImplementation).GetEnumerator();
    }

    public void Add(ICepProviderHandler item)
    {
        _listImplementation.Add(item);
    }

    public void Clear()
    {
        _listImplementation.Clear();
    }

    public bool Contains(ICepProviderHandler item)
    {
        return _listImplementation.Contains(item);
    }

    public void CopyTo(ICepProviderHandler[] array, int arrayIndex)
    {
        _listImplementation.CopyTo(array, arrayIndex);
    }

    public bool Remove(ICepProviderHandler item)
    {
        return _listImplementation.Remove(item);
    }

    public int Count => _listImplementation.Count;

    public bool IsReadOnly => _listImplementation.IsReadOnly;

    public int IndexOf(ICepProviderHandler item)
    {
        return _listImplementation.IndexOf(item);
    }

    public void Insert(int index, ICepProviderHandler item)
    {
        _listImplementation.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _listImplementation.RemoveAt(index);
    }

    public ICepProviderHandler this[int index]
    {
        get => _listImplementation[index];
        set => _listImplementation[index] = value;
    }

    # endregion
}