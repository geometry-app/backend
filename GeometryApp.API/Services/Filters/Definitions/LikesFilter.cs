namespace GeometryApp.API.Services.Filters.Definitions;

public class LikesFilter : IFilter
{
    public string Name => "likes";

    public string Field => "metaPreview.likes";

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Number, Name);
    }
}
