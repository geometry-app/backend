using System.Threading.Tasks;
using Folleach.Properties;
using GeometryApp.Common;
using GeometryApp.Proxy;
using GeometryDashAPI.Server.Queries;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations.Level;

public class LevelSearchExplorer(IPropertiesProvider provider, IProxyService proxyService, ResourceTranslator translator)
    : IExplorer<LevelSearchRequest>
{
    private readonly IPropertiesProvider provider = provider;
    private readonly IProxyService proxyService = proxyService;
    private readonly ResourceTranslator translator = translator;

    public async Task<ExploreResult> Explore(ExploreRequest<LevelSearchRequest> request, ILog log)
    {
        var proxy = await proxyService.TryGetProxy();
        if (!translator.TryGetUrl(request.Properties.Resource, out var url))
            return new ExploreResult(ExploreStatus.Error, message: $"can't translate resource '{request.Properties.Resource}' to url");
        var response = await proxy.DoRequest(url, x => x.SearchLevelsAsync(new GetLevelsQuery(request.Properties.SearchType)
        {
            Difficults = request.Properties.Difficulties,
            Page = request.Properties.Page,
            QueryString = request.Properties.SearchRequest ?? "-"
        }));
        return new ExploreResult(
            ExploreStatus.Success,
            value: response.ToUniversalJson(0, request.Properties));
    }
}
