namespace GeometryApp.Explorer;

public interface IExploreRequest
{
    string Type { get; }
    string Resource { get; set; }
}
