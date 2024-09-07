namespace GeometryApp.Services.Roulette.Repositories.Entities;

public class RouletteEntity
{
    public Guid RouletteId { get; set; }
    public int SequenceNumber { get; set; }
    public int LevelNumber { get; set; }
    public int LevelId { get; set; }
    public string? Server { get; set; }
    public string LevelName { get; set; } = null!;
    public int DemonDifficulty { get; set; }
    public bool Sealed { get; set; }
}
