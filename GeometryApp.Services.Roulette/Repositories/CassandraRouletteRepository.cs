using Cassandra.Data.Linq;
using Cassandra.Mapping;
using GeometryApp.Common;
using GeometryApp.Common.Configs;
using GeometryApp.Common.Models.Elastic.Levels;
using GeometryApp.Common.Models.Front;
using GeometryApp.Explorer;
using GeometryApp.Repositories.Cassandra;
using GeometryApp.Repositories.Elastic;
using GeometryApp.Services.Roulette.Repositories.Entities;
using Nest;

namespace GeometryApp.Services.Roulette.Repositories;

public class CassandraRouletteRepository : IRouletteRepository
{
    private readonly ElasticApp elastic;
    private readonly IMapper mapper;

    public CassandraRouletteRepository(GeometryAppCassandra cassandra, CassandraTopology topology, ElasticApp elastic)
    {
        this.elastic = elastic;
        var configuration = new MappingConfiguration().Define(new CassandraMappings(topology));
        var table = new Table<RouletteEntity>(cassandra.GetSession(), configuration);
        table.CreateIfNotExists();
        mapper = new Mapper(cassandra.GetSession(), configuration);
    }

    public async Task<Guid> Save(Roulette roulette)
    {
        var batch = mapper.CreateBatch();
        var sequenceNumber = 0;
        foreach (var sequence in roulette.Levels!)
        {
            var levelNumber = 0;
            foreach (var level in sequence.Levels!)
            {
                batch.Insert(new RouletteEntity()
                {
                    RouletteId = roulette.RouletteId,
                    SequenceNumber = sequenceNumber,
                    LevelNumber = levelNumber,
                    LevelId = level.Id,
                    Server = level.Server ?? Resources.GeometryDashServer,
                    LevelName = level.Name.ToString(),
                    DemonDifficulty = level.DemonDifficulty,
                    Sealed = false
                });
                levelNumber++;
            }
            sequenceNumber++;
        }

        await mapper.ExecuteAsync(batch);
        return roulette.RouletteId;
    }

    public async Task<Roulette> Get(Guid rouletteId)
    {
        var rawRoulette = (await mapper.FetchAsync<RouletteEntity>($"WHERE {nameof(RouletteEntity.RouletteId)} = ?", rouletteId)).ToArray();
        var roulette = new Roulette()
        {
            RouletteId = rouletteId,
        };
        var levels = await elastic
            .GetClient()
            .SearchAsync<LevelIndexFull>(x => x
                .Query(q => q
                    .Ids(i => i.Values(rawRoulette.Distinct().Select(l => new Id($"{l.Server ?? Resources.GeometryDashServer}_{l.LevelId}"))))
                )
                .Size(400)
            );
        var levelMap = levels.Hits.ToDictionary(x => x.Source.MetaPreview!.Id, x => x.Source);
        var sequence = new Dictionary<int, List<LevelPreviewDto>>();
        foreach (var item in rawRoulette)
        {
            if (!sequence.TryGetValue(item.SequenceNumber, out var batch))
                sequence[item.SequenceNumber] = batch = new List<LevelPreviewDto>();
            levelMap.TryGetValue(item.LevelId, out var info);
            var badges = info?.Badges?
                .Select(x => BadgeFormatter.Format(x.Key, x.Value as Dictionary<string, object>))
                .ToArray();
            batch.Insert(item.LevelNumber, new LevelPreviewDto()
            {
                Id = item.LevelId,
                Name = item.LevelName.ToHighlightString()!,
                DemonDifficulty = item.DemonDifficulty,
                Description = info?.MetaPreview?.Description.ToHighlightString(),
                Password = CryptExtensions.GetPasswordIfSetFromBase64(info?.MetaFull?.Password),
                Difficulty = info?.MetaPreview?.Difficulty ?? 0,
                DifficultyIcon = info?.MetaPreview?.DifficultyIcon ?? 0,
                IsDemon = info?.MetaPreview?.IsDemon ?? false,
                Server = info?.Server,
                Badges = badges
            });
        }

        roulette.Levels = sequence.OrderBy(x => x.Key).Select(x => new RouletteEntry() { Levels = x.Value.ToArray() }).ToList();
        return roulette;
    }

    public Task SealSequence(Guid rouletteId, int sequenceId)
    {
        throw new NotImplementedException();
    }
}
