namespace GeometryApp.Services.Roulette;

public class RouletteLevelWeights
{
    public float? Auto { get; set; }
    public float? Undef { get; set; }
    public float? Easy { get; set; }
    public float? Normal { get; set; }
    public float? Hard { get; set; }
    public float? Harder { get; set; }
    public float? Insane { get; set; }
    public float? EasyDemon { get; set; }
    public float? MediumDemon { get; set; }
    public float? HardDemon { get; set; }
    public float? InsaneDemon { get; set; }
    public float? ExtremeDemon { get; set; }

    public static RouletteLevelWeights Empty { get; } = new()
    {
        Auto = 0,
        Undef = 0,
        Easy = 0,
        Normal = 0,
        Hard = 0,
        Harder = 0,
        Insane = 0,
        EasyDemon = 0,
        MediumDemon = 0,
        HardDemon = 0,
        InsaneDemon = 0,
        ExtremeDemon = 0
    };
}
