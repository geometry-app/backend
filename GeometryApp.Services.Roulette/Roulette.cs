using GeometryApp.Common.Models.Front;

namespace GeometryApp.Services.Roulette;

public class Roulette
{
    public Guid RouletteId { get; set; }
    public string? Name { get; set; }
    public bool IsPublished { get; set; }
    public List<RouletteEntry>? Levels { get; set; }
}

public class RouletteEntry
{
    public LevelPreviewDto[]? Levels { get; set; }
}

public class RouletteLevel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int DemonDifficulty { get; set; }
}
