using Cassandra.Mapping;
using GeometryApp.Common.Configs;
using GeometryApp.Repositories.Entities;

namespace GeometryApp.Repositories
{
    public class GeometryAppMappings : Mappings
    {
        public string Keyspace { get; }

        public GeometryAppMappings(CassandraTopology config)
        {
            Keyspace = config.MainKeyspace;
            My<LevelEntity>()
                .TableName("levels_meta")
                .PartitionKey(x => x.Id)
                .ClusteringKey(x => x.CheckDate, SortOrder.Descending)
                .Column(x => x.Length, x => x.WithDbType<int>());

            My<ServiceEntity>()
                .TableName("services")
                .PartitionKey(x => x.ServiceName, x => x.Environment)
                .ClusteringKey(x => x.Version, SortOrder.Descending);
        }

        private Map<TPoco> My<TPoco>() => For<TPoco>().KeyspaceName(Keyspace);
    }
}
