using System.Linq;
using GeometryDashAPI.Server.Dtos;
using GeometryDashAPI.Server.Responses;

namespace GeometryApp.Common.Models.Elastic.Levels;

public class LevelMetaPreview
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int AuthorId { get; set; }
    public int AuthorUserId { get; set; }
    public string? AuthorName { get; set; }
    public string? Description { get; set; }
    public bool IsDemon { get; set; }
    public int DemonDifficulty { get; set; }
    public int DifficultyIcon { get; set; }
    public int Difficulty { get; set; }
    public int Version { get; set; }
    public int Download { get; set; }
    public int OfficialSong { get; set; }
    public int GameVersion { get; set; }
    public int Likes { get; set; }
    public int Length { get; set; }
    public int Stars { get; set; }
    public int FeatureScore { get; set; }
    public bool IsAuto { get; set; }
    public int CopiedId { get; set; }
    public bool TwoPlayer { get; set; }
    public int MusicId { get; set; }
    public string? MusicName { get; set; }
    public string? MusicAuthorName { get; set; }
    public string? MusicUrl { get; set; }
    public int Coins { get; set; }
    public bool IsConisVerified { get; set; }
    public int StarsRequest { get; set; }
    public bool IsEpic { get; set; }

    public static LevelMetaPreview From(LevelPageResponse page, LevelPreview level)
    {
        return new LevelMetaPreview()
        {
            Name = level.Name,
            Description = level.Description,
            AuthorId = page.Authors.FirstOrDefault(x => x.UserId == level.AuthorUserId)?.AccountId ?? 0,
            AuthorUserId = level.AuthorUserId,
            AuthorName = page.Authors.FirstOrDefault(x => x.UserId == level.AuthorUserId)?.UserName,
            Id = level.Id,
            IsDemon = level.Demon,
            DemonDifficulty = (int)level.DemonDifficulty,
            Difficulty = (int)level.Difficulty,
            DifficultyIcon = (int)level.DifficultyIcon,
            Version = level.Version,
            Download = level.Downloads,
            OfficialSong = level.OfficialSong,
            GameVersion = level.GameVersion,
            Likes = level.Likes,
            Length = (int)level.Length,
            Stars = level.Stars,
            FeatureScore = level.FeatureScore,
            IsAuto = level.Auto,
            CopiedId = level.CopiedId,
            TwoPlayer = level.TwoPlayer,
            MusicId = level.MusicId,
            MusicName = page.Musics?.FirstOrDefault(x => x.MusicId == level.MusicId)?.MusicName,
            MusicAuthorName = page.Musics?.FirstOrDefault(x => x.MusicId == level.MusicId)?.AuthorName,
            MusicUrl = page.Musics?.FirstOrDefault(x => x.MusicId == level.MusicId)?.Url,
            Coins = level.Coins,
            IsConisVerified = level.CoinsVerified,
            StarsRequest = level.StarsRequested,
            IsEpic = level.Epic,
        };
    }
}
