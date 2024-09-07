using GeometryDashAPI.Factories;
using GeometryDashAPI.Server;

namespace GeometryApp.Proxy;

public class ProxyFactory(ResponseFilter filter)
{
    private readonly ResponseFilter filter = filter;

    public GameClient CreateProxy(string proxy, string resource)
    {
        if (proxy == null)
            return new GameClient(network: new Network(resource, new DefaultHttpClientFactory(), filter.Filter));
        return new GameClient(network: new Network(resource, new ProxyHttpClientFactory(proxy, null, null), filter.Filter));
    }
}
