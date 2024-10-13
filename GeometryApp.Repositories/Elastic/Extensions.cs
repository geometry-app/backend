using System;
using System.Collections.Generic;
using System.Linq;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.Elastic.Levels;
using Nest;

namespace GeometryApp.Repositories.Elastic;

public static class Extensions
{
    public static QueryContainer ApplyQueryRequest(
        this QueryContainerDescriptor<LevelIndexFull> descriptor,
        PreparedRequest request,
        List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>? must = null,
        List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>? mustNot = null)
    {
        return descriptor.Bool(b =>
            {
                var list = new List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>()
                {
                    // some levels can be generated without required metaPreview field
                    w => w.Exists(e => e.Field(x => x.MetaPreview)),
                    // there is no active unofficial servers now
                    w => w.Term(e => e.Field(x => x.Server).Value("geometrydash"))
                };
                var notList = new List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>();

                foreach (var filter in request.Filters)
                {
                    var hasNot = (filter.Operator & InternalFilterOperator.Not) == InternalFilterOperator.Not;
                    if ((filter.Operator & InternalFilterOperator.Equals) == InternalFilterOperator.Equals)
                        (hasNot ? notList : list).Add(w => w.Terms(t => t.Field(filter.Field).Terms(filter.Values)));
                    if ((filter.Operator & InternalFilterOperator.Less) == InternalFilterOperator.Less)
                        (hasNot ? notList : list).Add(w => w.Range(t => t.Field(filter.Field).LessThan(double.Parse(filter.Values[0]))));
                    if ((filter.Operator & InternalFilterOperator.More) == InternalFilterOperator.More)
                        (hasNot ? notList : list).Add(w => w.Range(t => t.Field(filter.Field).GreaterThan(double.Parse(filter.Values[0]))));
                    if ((filter.Operator & InternalFilterOperator.Exists) == InternalFilterOperator.Exists)
                    {
                        foreach (var value in filter.Values)
                            (hasNot ? notList : list).Add(w => w.Exists(e => e.Field($"{filter.Field}.{value}")));
                    }
                }

                list.Add(w => w.MultiMatch(_ => new MultiMatchQuery()
                {
                    Query = request.Text,
                    Fields = SimpleSearchFields,
                    Lenient = true,
                    Operator = Operator.And
                }));

                return b
                    .Must(list.Concat(must ?? []))
                    .MustNot(notList.Concat(mustNot ?? []));
            }
        );
    }

    private static readonly string[] SimpleSearchFields =
    {
        "metaPreview.id",
        "metaPreview.name",
        "metaPreview.description",
        "metaFull.inGameText"
    };
}
