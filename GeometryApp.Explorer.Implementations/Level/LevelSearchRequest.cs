using System.Collections.Generic;
using GeometryDashAPI.Server.Enums;

namespace GeometryApp.Explorer.Implementations.Level;

public class LevelSearchRequest : IExploreRequest
{
    public const string RequestType = "search_levels";
    public string Type => RequestType;

    public string Resource { get; set; } = null!;
    public int Page { get; set; }
    public List<SearchDifficulty> Difficulties { get; set; } = new();
    public SearchType SearchType { get; set; } = new();
    public string? SearchRequest { get; set; }
}
