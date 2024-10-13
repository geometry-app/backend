using GeometryApp.Common.Filters;

namespace GeometryApp.Services.Roulette;

public class RouletteSessionPreview
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool Owner { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreateDt { get; set; }
    public QueryRequest? Request { get; set; }
    public RouletteLevelWeights? Weights { get; set; }
    public string Version { get; set; } = "v1";
}
