using GeometryApp.Common.Models.Elastic.Levels;

namespace GeometryApp.Common.Models.Front;

public static class Extensions
{
    public static LevelPreviewDto GetPreview(this LevelIndexFull level)
    {
        return new LevelPreviewDto()
        {
            Difficulty = level.MetaPreview?.Difficulty ?? 0,
            Id = level.MetaPreview!.Id,
            Name = level.MetaPreview.Name.ToHighlightString()!,
            Description = level.MetaPreview?.Description?.ToHighlightString(),
            DemonDifficulty = level.MetaPreview?.DemonDifficulty ?? 0,
            DifficultyIcon = level.MetaPreview?.DifficultyIcon ?? 0,
            IsDemon = level.MetaPreview?.IsDemon ?? false,
            Server = level.Server,
            Password = CryptExtensions.GetPasswordIfSetFromBase64(level.MetaFull?.Password),
        };
    }
}
