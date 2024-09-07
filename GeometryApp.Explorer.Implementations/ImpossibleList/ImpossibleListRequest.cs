using System.Text.Json.Serialization;

namespace GeometryApp.Explorer.Implementations.ImpossibleList;

[RequestType(RequestType)]
public class ImpossibleListRequest : IExploreRequest
{
    [JsonIgnore]
    public const string RequestType = "impossible_list";
    [JsonIgnore]
    public string Type => RequestType;

    public string Resource { get; set; } = null!;
    public int Page { get; set; }
}
