using System.Text.Json.Serialization;

namespace GeometryApp.Explorer.Implementations.PointCreate;

public class PointCreateRequest : IExploreRequest
{
    [JsonIgnore]
    public const string RequestType = "pointcreate_hardest";
    [JsonIgnore]
    public string Type => RequestType;

    public string Resource { get; set; } = null!;
}
