using System;
using System.Threading;
using System.Threading.Tasks;
using GeometryApp.Proxy.Context;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Proxy.Local;

public class LocalProxyService(TimeSpan? cooldown) : IProxyService
{
    private readonly TimeSpan? cooldown = cooldown;
    private DateTime lastAccess = DateTime.MinValue;
    private SemaphoreSlim semaphore = new(1, 1);

    public async Task<IRequestContext> TryGetProxy()
    {
        await semaphore.WaitAsync();
        if (cooldown != null && lastAccess + cooldown > DateTime.UtcNow)
            await Task.Delay(cooldown.Value);
        lastAccess = DateTime.UtcNow;
        semaphore.Release();
        return new LocalRequestContext(LogProvider.Get());
    }
}
