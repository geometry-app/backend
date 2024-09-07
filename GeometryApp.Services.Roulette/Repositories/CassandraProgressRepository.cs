using Cassandra.Mapping;
using GeometryApp.Common.Configs;
using GeometryApp.Repositories;
using GeometryApp.Repositories.Cassandra;
using GeometryApp.Services.Roulette.Repositories.Entities;

namespace GeometryApp.Services.Roulette.Repositories;

public class CassandraProgressRepository : IProgressRepository
{
    private readonly IMapper mapper;

    public CassandraProgressRepository(GeometryAppCassandra cassandra, CassandraTopology topology)
    {
        mapper = cassandra.InitMapper<ProgressEntity>(new CassandraMappings(topology));
    }

    public async Task Set(Guid rouletteId, string sessionId, int sequenceNumber, int levelId, int progress)
    {
        await mapper.InsertAsync(new ProgressEntity()
        {
            RouletteId = rouletteId,
            SessionId = sessionId,
            SequenceNumber = sequenceNumber,
            LevelId = levelId,
            Progress = progress
        });
    }

    public async Task<IEnumerable<ProgressEntry>> Get(Guid rouletteId, string sessionId)
    {
        var result = await mapper.FetchAsync<ProgressEntity>(
            $"WHERE {nameof(ProgressEntity.RouletteId)} = ? AND {nameof(ProgressEntity.SessionId)} = ?",
            rouletteId, sessionId);
        return result.Select(x => new ProgressEntry()
        {
            LevelId = x.LevelId,
            Progress = x.Progress,
            SequenceNumber = x.SequenceNumber
        });
    }
}
