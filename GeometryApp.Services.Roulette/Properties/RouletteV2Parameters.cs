using GeometryApp.Common.Filters;

namespace GeometryApp.Services.Roulette.Properties;

public class RouletteV2Parameters
{
    public RouletteLevelWeights Weights { get; set; }
    public QueryRequest Request { get; set; }
    public string Version { get; set; } = "v1";
}
