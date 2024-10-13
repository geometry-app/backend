using GeometryApp.Common.Filters;

namespace GeometryApp.Services.Roulette;

public class RouletteSession
{
    public string SessionId { get; set; } = null!;
    public bool IsStarted { get; set; }
    public Roulette Roulette { get; set; } = null!;
    public IEnumerable<ProgressEntry> Progress { get; set; } = null!;
    public QueryRequest? Request { get; set; }
    public RouletteLevelWeights? Weights { get; set; }
    public string Version { get; set; } = "v1";
}
