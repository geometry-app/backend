using System.Linq;
using System.Threading.Tasks;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.Elastic.Levels;
using Nest;

namespace GeometryApp.Repositories.Elastic;

public class IndexElasticRepository : IIndexRepository
{
    private readonly ElasticApp elastic;

    public IndexElasticRepository(ElasticApp elastic)
    {
        this.elastic = elastic;
    }

    public async Task<ISearchResponse<LevelIndexFull>> SimpleSearch(string query, int skip = 0, int take = 10)
    {
        var response = await elastic.GetClient()
            .SearchAsync<LevelIndexFull>(d => d
                .Query(q => q
                    .MultiMatch(s => new MultiMatchQuery()
                    {
                        Query = query,
                        Fields = SimpleSearchFields,
                        Lenient = true,
                        Operator = Operator.And
                    }))
                .Highlight(h => h
                    .Fields(doc => doc.Field(x => x.MetaPreview.Name), doc => doc.Field(x => x.MetaPreview.Description)))
                .Skip(skip)
                .Take(take))
            .ConfigureAwait(false);
        return response;
    }

    public Task<ISearchResponse<LevelIndexFull>> AdvanceSearch(PreparedRequest request, int skip = 0, int take = 10)
    {
        var client = elastic.GetClient();
        return client.SearchAsync<LevelIndexFull>(d => d
            .Query(q => q.ApplyQueryRequest(request))
            .Skip(skip)
            .Take(take)
        );
    }

    public async Task<ISearchResponse<LevelIndexFull>> LuckySearch()
    {
        var response = await elastic.GetClient()
            .SearchAsync<LevelIndexFull>(d => d
                .Query(q => q
                    .FunctionScore(s => s
                        .Query(sq => sq
                            .MatchAll()
                        )
                        .Boost(2)
                        .Functions(Enumerable.Range(0, 6).Select(_ => new RandomScoreFunction()))
                        .BoostMode(FunctionBoostMode.Multiply)
                    )
                )
                .Take(1))
            .ConfigureAwait(false);
        return response;
    }

    private static readonly string[] SimpleSearchFields =
    {
        "metaPreview.id",
        "metaPreview.name",
        "metaPreview.description",
        "metaFull.inGameText"
    };
}
