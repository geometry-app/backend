using Cassandra.Data.Linq;
using Cassandra.Mapping;
using GeometryApp.Repositories.Cassandra;

namespace GeometryApp.Repositories;

public static class GeometryAppCassandraExtensions
{
    public static IMapper InitMapper<T>(this GeometryAppCassandra cassandra, Mappings mappings)
    {
        var configuration = new MappingConfiguration().Define(mappings);
        var table = new Table<T>(cassandra.GetSession(), configuration);
        table.CreateIfNotExists();
        return new Mapper(cassandra.GetSession(), configuration);
    }
}
