using GeometryApp.Common.Models.GeometryDash;
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

    public static (bool demon, InternalDifficultyIcon difficulty)? GetRandomV2(this RouletteLevelWeights weights)
    {
        var sum = (weights.EasyDemon ?? 0)
                  + (weights.MediumDemon ?? 0)
                  + (weights.HardDemon ?? 0)
                  + (weights.InsaneDemon ?? 0)
                  + (weights.ExtremeDemon ?? 0)
                  + (weights.Auto ?? 0)
                  + (weights.Undef ?? 0)
                  + (weights.Easy ?? 0)
                  + (weights.Normal ?? 0)
                  + (weights.Hard ?? 0)
                  + (weights.Harder ?? 0)
                  + (weights.Insane ?? 0);
        var cumulative = 0f;
        var value = random.NextSingle();
        var pointer = value * sum;
        if (TrySelect(pointer, weights.EasyDemon, ref cumulative))
            return (true, InternalDifficultyIcon.Easy);
        if (TrySelect(pointer, weights.MediumDemon, ref cumulative))
            return (true, InternalDifficultyIcon.Normal);
        if (TrySelect(pointer, weights.HardDemon, ref cumulative))
            return (true, InternalDifficultyIcon.Hard);
        if (TrySelect(pointer, weights.InsaneDemon, ref cumulative))
            return (true, InternalDifficultyIcon.Harder);
        if (TrySelect(pointer, weights.ExtremeDemon, ref cumulative))
            return (true, InternalDifficultyIcon.Insane);
        if (TrySelect(pointer, weights.Auto, ref cumulative))
            return (false, InternalDifficultyIcon.Auto);
        if (TrySelect(pointer, weights.Undef, ref cumulative))
            return (false, InternalDifficultyIcon.Undef);
        if (TrySelect(pointer, weights.Easy, ref cumulative))
            return (false, InternalDifficultyIcon.Easy);
        if (TrySelect(pointer, weights.Normal, ref cumulative))
            return (false, InternalDifficultyIcon.Normal);
        if (TrySelect(pointer, weights.Hard, ref cumulative))
            return (false, InternalDifficultyIcon.Hard);
        if (TrySelect(pointer, weights.Harder, ref cumulative))
            return (false, InternalDifficultyIcon.Harder);
        if (TrySelect(pointer, weights.Insane, ref cumulative))
            return (false, InternalDifficultyIcon.Insane);
        return null;
    }

    private static bool TrySelect(float pointer, float? weight, ref float cumulative)
    {
        cumulative += weight ?? 0;
        if (pointer < cumulative)
            return true;
        return false;
    }
}
