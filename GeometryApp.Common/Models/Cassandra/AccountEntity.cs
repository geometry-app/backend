using System;

namespace GeometryApp.Common.Models.Cassandra;

public class AccountEntity
{
    public int AccountId;
    public DateTime ActualDate;

    public string Name = null!;
    public int UserId;
    public int Starts;
    public int Demons;
    public int Rank;
    public int Highlight;
    public int CreatorPoints;
    public int IconPrev;
    public int Color1;
    public int Color2;
    public int SecretCoins;
    public int IconType;
    public int Special;
    public int UserCoins;
    public int MessageState;
    public int FriendsState;
    public string? YouTubeId;
    public int CubeId;
    public int ShipId;
    public int BallId;
    public int BirdId;
    public int WaveId;
    public int RobotId;
    public int StreakId;
    public int GlowId;
    public bool IsRegistered;
    public int GlobalRank;
    public int UsFriendState;
    public int Messages;
    public int FriendRequests;
    public bool NewFriends;
    public int NewFriendRequests;
    public int TimeSinceSubmittedLevel;
    public int SpiderId;
    public string? TwitterId;
    public string? TwitchId;
    public int Diamonds;
    public int ExplosionId;
    public int Moderator;
    public int CommentHistoryState;
}
