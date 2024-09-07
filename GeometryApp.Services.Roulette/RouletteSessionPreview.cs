namespace GeometryApp.Services.Roulette;

public class RouletteSessionPreview
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool Owner { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreateDt { get; set; }
}
