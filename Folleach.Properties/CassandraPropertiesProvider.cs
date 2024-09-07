using System.Reflection;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;

namespace Folleach.Properties;

public class CassandraPropertiesProvider : IPropertiesProvider
{
    private readonly IMapper mapper;

    public CassandraPropertiesProvider(ISession session, string keyspace, string tableName)
    {
        session.CreateKeyspaceIfNotExists(keyspace);
        var configuration = new MappingConfiguration().Define(new CassandraMapping(keyspace, tableName));
        var table = new Table<PropertyEntity>(session, configuration);
        table.CreateIfNotExists();
        mapper = new Mapper(session, configuration);
    }

    public async Task<T> Get<T>(string scope, string id)
    {
        var properties = await mapper.FetchAsync<PropertyEntity>($"WHERE {nameof(PropertyEntity.Scope).ToLower()} = ? AND {nameof(PropertyEntity.Id).ToLower()} = ?", scope, id);
        var dictionary = properties.ToDictionary(x => x.Name, x => x.Value);
        var instance = Activator.CreateInstance<T>();
        foreach (var property in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetCustomAttribute<IgnorePropertyAttribute>() != null)
                continue;
            if (dictionary.TryGetValue(property.Name, out var value))
                property.SetValue(instance, value);
        }

        return instance;
    }

    public async Task Insert<T>(string scope, string id, T values)
    {
        var utc = DateTime.UtcNow;
        var properties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.GetCustomAttribute<IgnorePropertyAttribute>() == null)
            .Select(x => (x.Name, (string)x.GetValue(values)!))
            .Select(x => new PropertyEntity()
            {
                Id = id,
                Scope = scope,
                Name = x.Name,
                Value = x.Item2,
                UpdateDt = utc
            });
        var batch = mapper.CreateBatch();
        foreach (var property in properties)
            batch.Insert(property);
        await mapper.ExecuteAsync(batch);
    }

    public Task Delete(string scope, string id)
    {
        throw new NotImplementedException();
    }
}
