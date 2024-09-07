using System.Threading.Tasks;
using GeometryApp.Proxy.Context;

namespace GeometryApp.Proxy;

public interface IProxyService
{
    Task<IRequestContext> TryGetProxy();
}
