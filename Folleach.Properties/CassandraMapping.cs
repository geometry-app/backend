using Cassandra.Mapping;

namespace Folleach.Properties;

public class CassandraMapping : Mappings
{
    public CassandraMapping(string keyspace, string table)
    {
        For<PropertyEntity>()
            .KeyspaceName(keyspace)
            .TableName(table)
            .PartitionKey(x => x.Scope, x => x.Id)
            .ClusteringKey(x => x.Name);
    }
}
