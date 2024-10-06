using GeometryDashAPI.Server.Enums;

namespace GeometryApp.Services.Roulette;

public static class DemonWeightExtensions
{
    private static readonly Random random = new();

    public static DemonDifficulty GetRandom(this RouletteLevelWeights weights)
    {
        var sum = weights.EasyDemon ?? 0
                  + weights.MediumDemon ?? 0
                  + weights.HardDemon ?? 0
                  + weights.InsaneDemon ?? 0
                  + weights.ExtremeDemon ?? 0;
        var cumulative = 0f;
        var value = random.NextSingle();
        var pointer = value * sum;
        if (TrySelect(pointer, weights.EasyDemon, ref cumulative))
            return DemonDifficulty.Easy;
        if (TrySelect(pointer, weights.MediumDemon, ref cumulative))
            return DemonDifficulty.Medium;
        if (TrySelect(pointer, weights.HardDemon, ref cumulative))
            return DemonDifficulty.Hard;
        if (TrySelect(pointer, weights.InsaneDemon, ref cumulative))
            return DemonDifficulty.Insane;
        if (TrySelect(pointer, weights.ExtremeDemon, ref cumulative))
            return DemonDifficulty.Extreme;
        return DemonDifficulty.Extreme;
    }

    private static bool TrySelect(float pointer, float? weight, ref float cumulative)
    {
        cumulative += weight ?? 0;
        if (pointer < cumulative)
            return true;
        return false;
    }
}
