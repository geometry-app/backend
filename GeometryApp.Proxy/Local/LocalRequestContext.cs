using System;
using System.Text;
using System.Threading.Tasks;
using GeometryApp.Proxy.Context;
using GeometryDashAPI;
using GeometryDashAPI.Factories;
using GeometryDashAPI.Server;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Proxy.Local;

public class LocalRequestContext(ILog log) : IRequestContext
{
    private readonly ResponseFilter responseFilter = new ResponseFilter(log);

    public async Task<ServerResponse<T>> DoRequest<T>(string server, Func<GameClient, Task<ServerResponse<T>>> action) where T : IGameObject
    {
        var current = new GameClient(network: new Network(
            server,
            new DefaultHttpClientFactory(),
            responseFilter.Filter)
        );
        return await action(current);
    }
}
