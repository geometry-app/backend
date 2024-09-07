namespace GeometryApp.Common.Models.Front;

public enum PreviewType
{
    None = 0,
    Level
}

public class PreviewEntity
{
    public PreviewType Type { get; set; }
}
