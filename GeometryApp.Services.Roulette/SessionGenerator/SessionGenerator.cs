namespace GeometryApp.Services.Roulette.SessionGenerator;

public class SessionGenerator : IRouletteSessionGenerator
{
    public string GenerateSessionId()
    {
        return $"{ReplacedGuid()}{ReplacedGuid()}{ReplacedGuid()}";
    }

    private static string ReplacedGuid()
    {
        return Guid.NewGuid().ToString().Replace("-", string.Empty);
    }
}
