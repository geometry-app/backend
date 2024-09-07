using System.Collections.Generic;

namespace GeometryApp.Common.Models.Elastic.Levels;

public class LevelIndexFull
{
    public LevelMetaPreview? MetaPreview { get; set; }
    public LevelMetaFull? MetaFull { get; set; }
    public string? Server { get; set; }
    public Dictionary<string, object>? Badges { get; set; }
}
