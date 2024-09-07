using Cassandra.Mapping;
using GeometryApp.Common.Configs;
using GeometryApp.Services.Roulette.Repositories.Entities;

namespace GeometryApp.Services.Roulette.Repositories;

public class CassandraMappings : Mappings
{
    public CassandraMappings(CassandraTopology topology)
    {
        For<RouletteEntity>()
            .KeyspaceName(topology.MainKeyspace)
            .TableName("roulette")
            .PartitionKey(x => x.RouletteId)
            .ClusteringKey(
                Tuple.Create(nameof(RouletteEntity.SequenceNumber), SortOrder.Ascending),
                Tuple.Create(nameof(RouletteEntity.LevelNumber), SortOrder.Ascending));

        For<ProgressEntity>()
            .KeyspaceName(topology.MainKeyspace)
            .TableName("roulette_progress")
            .PartitionKey(x => x.SessionId, x => x.RouletteId)
            .ClusteringKey(x => x.SequenceNumber, SortOrder.Ascending)
            .ClusteringKey(x => x.LevelId, SortOrder.Ascending);

        For<RouletteOwnerEntity>()
            .KeyspaceName(topology.MainKeyspace)
            .TableName("roulette_owners")
            .PartitionKey(x => x.SessionId)
            .ClusteringKey(x => x.RouletteId);
    }
}
