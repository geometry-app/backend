namespace GeometryApp.API.Services.Filters.Definitions;

public class LengthFilter : IFilter
{
    public string Name => "length";

    public string Field => "metaPreview.length";

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Term, Name);
    }
}
