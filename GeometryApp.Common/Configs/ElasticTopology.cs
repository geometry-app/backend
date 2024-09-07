namespace GeometryApp.Common.Configs;

public class ElasticTopology
{
    public string IndexPrefix { get; set; } = null!;

    public string GetIndex(string indexName)
    {
        return $"{IndexPrefix}_{indexName}";
    }
}
