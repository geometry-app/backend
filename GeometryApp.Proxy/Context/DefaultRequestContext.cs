using System;
using System.Threading.Tasks;
using GeometryDashAPI;
using GeometryDashAPI.Server;

namespace GeometryApp.Proxy.Context;

public class DefaultRequestContext : IRequestContext
{
    private readonly GameClient client;
    private bool used = false;

    public DefaultRequestContext(GameClient client)
    {
        this.client = client;
    }

    public async Task<ServerResponse<T>> DoRequest<T>(string _, Func<GameClient, Task<ServerResponse<T>>> action) where T : IGameObject
    {
        lock (this)
        {
            if (used)
                throw new InvalidOperationException("context already used");
            used = true;
        }
        return await action(client);
    }
}
