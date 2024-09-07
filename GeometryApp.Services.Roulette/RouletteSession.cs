namespace GeometryApp.Services.Roulette;

public class RouletteSession
{
    public string SessionId { get; set; } = null!;
    public bool IsStarted { get; set; }
    public Roulette Roulette { get; set; } = null!;
    public IEnumerable<ProgressEntry> Progress { get; set; } = null!;
}
