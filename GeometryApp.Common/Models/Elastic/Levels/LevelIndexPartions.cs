using System.Collections.Generic;

namespace GeometryApp.Common.Models.Elastic.Levels;

public class LevelIndexMetaFull
{
    public string? Server { get; set; }
    public LevelMetaFull? MetaFull { get; set; }
}

public class LevelIndexMetaPreview
{
    public string? Server { get; set; }
    public LevelMetaPreview? MetaPreview { get; set; }
}

public class LevelIndexMetaFullAndPreview
{
    public string? Server { get; set; }
    public LevelMetaFull? MetaFull { get; set; }
    public LevelMetaPreview? MetaPreview { get; set; }
}

public class LevelIndexBadges
{
    public Dictionary<string, object>? Badges { get; set; }
}

