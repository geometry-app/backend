using System;
using System.Threading.Tasks;
using GeometryApp.Common;
using GeometryApp.Proxy;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations.Level;

public class LevelExplorer(IProxyService service) : IExplorer<LevelExploreRequest>
{
    private readonly IProxyService service = service;

    public async Task<ExploreResult> Explore(ExploreRequest<LevelExploreRequest> request, ILog log)
    {
        var id = request.Properties.Id;
        byte[] json = null!;
        while (json == null)
        {
            try
            {
                var proxy = await service.TryGetProxy();
                var response = await proxy.DoRequest(request.Properties.Resource, x => x.DownloadLevelAsync(id));
                json = response.ToUniversalJson(id, request.Properties);
            }
            catch (Exception e)
            {
                log.Warn(e, $"exception raised: {e.Message}");
            }
        }
        return json.AsSuccess();
    }
}
