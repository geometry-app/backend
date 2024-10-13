using System.Collections.Generic;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services.Filters.Definitions;

public class DownloadsFilter : IFilter
{
    public const string Field = "metaPreview.download";

    public string Name => "downloads";

    public FilterDefinition GetDefinition() => new(FilterType.Number, Name);

    public IEnumerable<InternalFilter> Enrich(Filter item)
    {
        yield return new InternalFilter(Field, [item.Value], (InternalFilterOperator)item.Operator);
    }
}
