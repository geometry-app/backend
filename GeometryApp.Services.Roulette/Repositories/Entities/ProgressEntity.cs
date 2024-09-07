namespace GeometryApp.Services.Roulette.Repositories.Entities;

public class ProgressEntity
{
    public string SessionId { get; set; } = null!;
    public Guid RouletteId { get; set; }
    public int SequenceNumber { get; set; }
    public int LevelId { get; set; }
    public int Progress { get; set; }
}
