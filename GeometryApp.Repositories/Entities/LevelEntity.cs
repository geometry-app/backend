using System;
using GeometryDashAPI.Server.Enums;

namespace GeometryApp.Repositories.Entities
{
    public class LevelEntity
    {
        public int Id;
        public DateTime CheckDate;
        public Guid StorageId;
        public int StatusCode;
        public int HttpStatusCode;
        public int AuthorUserId;
        public string Name;
        public string Description;
        public byte[] LevelHash;
        public int Likes;
        public int Downloads;
        public int Difficulty;
        public int SlaveDifficulty;
        public int Version;
        public string RawDate;
        public string RawUpdateDate;
        public bool Epic;
        public bool Auto;
        public int Coins;
        public bool IsDemon;
        public LengthType Length;
        public int Objects;
        public int Stars;
        public bool CoinsVerified;
        public int CopiedId;
        public int DemonDifficulty;
        public int? EditorTime;
        public int? EditorTimeSum;
        public string Extra;
        public int FeatureScore;
        public int GameVersion;
        public bool LowDetail;
        public int MusicId;
        public int OfficialSong;
        public string RawPassword;
        public int StarsRequested;
        public bool TwoPlayer;
    }
}
