using System;
using System.Threading.Tasks;
using GeometryApp.Common;
using GeometryApp.Proxy;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations.Account;

public class AccountExplorer(IProxyService proxyService) : IExplorer<AccountRequest>
{
    private readonly IProxyService proxyService = proxyService;

    public async Task<ExploreResult> Explore(ExploreRequest<AccountRequest> request, ILog log)
    {
        var id = request.Properties.Id;
        byte[] json = null!;
        while (json == null)
        {
            try
            {
                var proxy = await proxyService.TryGetProxy();
                var response = await proxy.DoRequest(request.Properties.Resource, x => x.GetAccountAsync(id));
                json = response.ToUniversalJson(0, request.Properties);
            }
            catch (Exception e)
            {
                log.Warn(e, $"exception raised: {e.Message}");
            }
        }
        return json.AsSuccess();
    }
}
