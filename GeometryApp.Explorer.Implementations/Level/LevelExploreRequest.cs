namespace GeometryApp.Explorer.Implementations.Level;

public class LevelExploreRequest : IExploreRequest
{
    public const string RequestType = "download_level";
    public string Type => RequestType;

    public int Id { get; set; }
    public string Resource { get; set; } = null!;
}
