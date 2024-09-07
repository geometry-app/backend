namespace GeometryApp.API.Services.Filters;

public interface IFilter
{
    public string Name { get; }
    public string Field { get; }
    FilterDefinition GetDefinition();
}
