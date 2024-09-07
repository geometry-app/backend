namespace GeometryApp.Services.Roulette.Repositories.Entities;

public class RouletteOwnerEntity
{
    public string SessionId { get; set; } = null!;
    public bool Owner { get; set; }
    public Guid RouletteId { get; set; }
}
