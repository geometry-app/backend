using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using GeometryApp.Common.Configs;
using GeometryApp.Common.Models.Elastic.Levels;
using GeometryApp.Repositories.Topologies;
using Nest;

namespace GeometryApp.Repositories.Elastic;

public class ElasticApp
{
    private readonly IElasticClient client;
    
    public ElasticApp(ElasticConfig config)
    {
        var pool = new SingleNodeConnectionPool(new Uri($"http://{config.Host}:{config.Port}"));
        var settings = new ConnectionSettings(pool).EnableDebugMode();
        client = new ElasticClient(settings);
    }

    public async Task ApplyIndices(ITopologyProvider provider)
    {
        var properties = await provider.Get<ElasticProperties>();
        if (properties == null || properties.LevelsIndex == null)
            throw new InvalidOperationException("Elastic topology is not set");
        client.ConnectionSettings.DefaultIndices.Add(typeof(LevelIndexFull), $"prod_{properties.LevelsIndex}");
    }

    public IElasticClient GetClient() => client;
}
