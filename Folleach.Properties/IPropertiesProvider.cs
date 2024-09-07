namespace Folleach.Properties;

public interface IPropertiesProvider
{
    Task<T> Get<T>(string scope, string id);
    Task Insert<T>(string scope, string id, T values);
    Task Delete(string scope, string id);
}
