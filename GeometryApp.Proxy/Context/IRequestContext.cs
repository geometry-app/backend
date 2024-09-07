using System;
using System.Threading.Tasks;
using GeometryDashAPI;
using GeometryDashAPI.Server;

namespace GeometryApp.Proxy.Context;

public interface IRequestContext
{
    Task<ServerResponse<T>> DoRequest<T>(string server, Func<GameClient, Task<ServerResponse<T>>> action) where T : IGameObject;
}
