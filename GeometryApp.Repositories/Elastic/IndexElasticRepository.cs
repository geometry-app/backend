using System;
using System.Collections.Generic;
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

    public Task<ISearchResponse<LevelIndexFull>> AdvanceSearch(string text, InternalFilter[] filters, int skip = 0, int take = 10)
    {
        var client = elastic.GetClient();
        return client.
            SearchAsync<LevelIndexFull>(d => d
            .Query(q => q
                .Bool(b =>
                {
                    var list = new List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>();
                    var notList = new List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>();

                    foreach (var filter in filters)
                    {
                        var hasNot = (filter.Operator & FilterOperator.Not) == FilterOperator.Not;
                        if ((filter.Operator & FilterOperator.Equals) == FilterOperator.Equals)
                            (hasNot ? notList : list).Add(w => w.Term(t => t.Field(filter.Field).Value(filter.Value)));
                        if ((filter.Operator & FilterOperator.Less) == FilterOperator.Less)
                            (hasNot ? notList : list).Add(w => w.Range(t => t.Field(filter.Field).LessThan(double.Parse(filter.Value))));
                        if ((filter.Operator & FilterOperator.More) == FilterOperator.More)
                            (hasNot ? notList : list).Add(w => w.Range(t => t.Field(filter.Field).GreaterThan(double.Parse(filter.Value))));
                    }

                    list.Add(w => w.MultiMatch(_ => new MultiMatchQuery()
                    {
                        Query = text,
                        Fields = SimpleSearchFields,
                        Lenient = true,
                        Operator = Operator.And
                    }));

                    return b.Must(list).MustNot(notList);
                }))
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
