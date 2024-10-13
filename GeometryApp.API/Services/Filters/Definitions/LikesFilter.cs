using System.Collections.Generic;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services.Filters.Definitions;

public class LikesFilter : IFilter
{
    public const string Field = "metaPreview.likes";

    public string Name => "likes";

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Number, Name);
    }

    public IEnumerable<InternalFilter> Enrich(Filter item)
    {
        yield return new InternalFilter(Field, [item.Value], (InternalFilterOperator)item.Operator);
    }
}
