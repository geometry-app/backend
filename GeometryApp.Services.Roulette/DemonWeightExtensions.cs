using GeometryDashAPI.Server.Enums;

namespace GeometryApp.Services.Roulette;

public static class DemonWeightExtensions
{
    private static readonly Random random = new();

    public static DemonDifficulty GetRandom(this DemonWeights weights)
    {
        var sum = weights.EasyDemon
                  + weights.MediumDemon
                  + weights.HardDemon
                  + weights.InsaneDemon
                  + weights.ExtremeDemon;
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

    private static bool TrySelect(float pointer, float weight, ref float cumulative)
    {
        cumulative += weight;
        if (pointer < cumulative)
            return true;
        return false;
    }
}
