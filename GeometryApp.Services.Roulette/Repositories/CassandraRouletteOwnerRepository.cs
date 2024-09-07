using Cassandra.Mapping;
using GeometryApp.Common.Configs;
using GeometryApp.Repositories;
using GeometryApp.Repositories.Cassandra;
using GeometryApp.Services.Roulette.Repositories.Entities;

namespace GeometryApp.Services.Roulette.Repositories;

public class CassandraRouletteOwnerRepository : IRouletteOwnerRepository
{
    private readonly IMapper mapper;

    public CassandraRouletteOwnerRepository(GeometryAppCassandra cassandra, CassandraTopology topology)
    {
        mapper = cassandra.InitMapper<RouletteOwnerEntity>(new CassandraMappings(topology));
    }
    
    public async Task Put(string sessionId, bool owner, Guid rouletteId)
    {
        await mapper.InsertAsync(new RouletteOwnerEntity()
        {
            SessionId = sessionId,
            Owner = owner,
            RouletteId = rouletteId
        });
    }

    public async Task<IEnumerable<RouletteOwnerEntity>> GetAll(string sessionId)
        => await mapper.FetchAsync<RouletteOwnerEntity>($"WHERE {nameof(RouletteOwnerEntity.SessionId)} = ?", sessionId);
}
