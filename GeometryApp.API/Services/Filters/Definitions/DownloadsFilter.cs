using System;

namespace GeometryApp.API.Services.Filters.Definitions;

public class DownloadsFilter : IFilter
{
    public string Name => "downloads";

    public string Field => "metaPreview.download";

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Number, Name);
    }
}
