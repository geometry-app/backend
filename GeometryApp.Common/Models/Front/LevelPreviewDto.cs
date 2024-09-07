using System.Runtime.Serialization;
using GeometryDashAPI.Server.Enums;

namespace GeometryApp.Common.Models.Front;

[DataContract]
public class LevelPreviewDto : PreviewEntity
{
    [DataMember] public HighlightString Name { get; set; } = null!;
    [DataMember] public int Id { get; set; }
    [DataMember] public HighlightString? Description { get; set; }
    [DataMember] public int Difficulty { get; set; }
    [DataMember] public int DemonDifficulty { get; set; }
    [DataMember] public int DifficultyIcon { get; set; }
    [DataMember] public bool IsDemon { get; set; }
    [DataMember] public string? Password { get; set; }
    [DataMember] public string[]? NotFound { get; set; }
    [DataMember] public string? Server { get; set; }
    [DataMember] public Badge[]? Badges { get; set; }
    [DataMember] public int? Likes { get; set; }
    [DataMember] public LengthType? Length { get; set; }
}
