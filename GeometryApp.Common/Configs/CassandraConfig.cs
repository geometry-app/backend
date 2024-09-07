namespace GeometryApp.Common.Configs;

public class CassandraConfig
{
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string[] ContactPoints { get; set; } = null!;
}
