namespace GeometryApp.Common.Models.Elastic.Accounts;

public class AccountIndexFull
{
    public int AccountId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public int Starts { get; set; }
    public int Demons { get; set; }
    public int Rank { get; set; }
    public int CreatorPoints { get; set; }
    public int IconInPreview { get; set; }
    public int Color1 { get; set; }
    public int Color2 { get; set; }
    public int SecretCoins { get; set; }
    public int IconType { get; set; }
    public int Special { get; set; }
    public int UserCoins { get; set; }
    public int Diamonds { get; set; }

    public int MessageState { get; set; }
    public int FriendState { get; set; }

    public int CubeId { get; set; }
    public int ShipId { get; set; }
    public int BallId { get; set; }
    public int BirdId { get; set; }
    public int WaveId { get; set; }
    public int RobotId { get; set; }
    public int SpiderId { get; set; }
    public int StreakId { get; set; }
    public int GlowId { get; set; }
    public int ExplosionId { get; set; }
    public bool IsRegistered { get; set; }
    public int GlobalRank { get; set; }
    public int Moderator { get; set; }

    public string? YouTubeId { get; set; }
    public string? TwitterId { get; set; }
    public string? TwitchId { get; set; }

}
