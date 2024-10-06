using System;
using System.Collections.Generic;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.Elastic.Levels;
using Nest;

namespace GeometryApp.Repositories.Elastic;

public static class Extensions
{
    public static QueryContainer ApplyQueryRequest(this QueryContainerDescriptor<LevelIndexFull> descriptor, PreparedRequest request)
    {
        return descriptor.Bool(b =>
            {
                var list = new List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>()
                {
                    // some levels can be generated without required metaPreview field
                    w => w.Exists(e => e.Field(x => x.MetaPreview))
                };
                var notList = new List<Func<QueryContainerDescriptor<LevelIndexFull>, QueryContainer>>();

                foreach (var filter in request.Filters)
                {
                    var hasNot = (filter.Operator & InternalFilterOperator.Not) == InternalFilterOperator.Not;
                    if ((filter.Operator & InternalFilterOperator.Equals) == InternalFilterOperator.Equals)
                        (hasNot ? notList : list).Add(w => w.Term(t => t.Field(filter.Field).Value(filter.Values)));
                    if ((filter.Operator & InternalFilterOperator.Less) == InternalFilterOperator.Less)
                        (hasNot ? notList : list).Add(w => w.Range(t => t.Field(filter.Field).LessThan(double.Parse(filter.Values[0]))));
                    if ((filter.Operator & InternalFilterOperator.More) == InternalFilterOperator.More)
                        (hasNot ? notList : list).Add(w => w.Range(t => t.Field(filter.Field).GreaterThan(double.Parse(filter.Values[0]))));
                    if ((filter.Operator & InternalFilterOperator.Exists) == InternalFilterOperator.Exists)
                        (hasNot ? notList : list).Add(w => w.Exists(e => e.Field($"{filter.Field}.{filter.Values[0]}")));
                }

                list.Add(w => w.MultiMatch(_ => new MultiMatchQuery()
                {
                    Query = request.Text,
                    Fields = SimpleSearchFields,
                    Lenient = true,
                    Operator = Operator.And
                }));

                return b.Must(list).MustNot(notList);
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
