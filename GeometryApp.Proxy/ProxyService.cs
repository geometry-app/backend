using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeometryApp.Proxy.Context;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Proxy;

public class ProxyObserver : IObserver<string>
{
    private readonly ResponseFilter filter;
    private readonly ILog log;
    private readonly ProxyFactory factory;
    private List<ProxyContext> proxies = new();

    public ProxyObserver(ResponseFilter filter, ILog log)
    {
        this.filter = filter;
        this.log = log;
        factory = new ProxyFactory(new ResponseFilter(log));
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(string value)
    {
    }

    public (ProxyContext ctx, DateTime lastUsage) GetProxy()
    {
        ProxyContext ctx;
        DateTime lastUsage;
        lock (proxies)
        {
            ctx = proxies.MinBy(x => x.LastUsage)!;
            lastUsage = ctx.LastUsage;
            ctx.LastUsage = DateTime.UtcNow;
        }

        return (ctx, lastUsage);
    }
}

public class ProxyService : IProxyService
{
    private readonly Func<DateTime, TimeSpan> shouldWait;
    private readonly ProxyObserver proxies;

    public ProxyService(IObservable<string> proxies, ResponseFilter responseFilter, Func<DateTime, TimeSpan> shouldWait, ILog log)
    {
        this.shouldWait = shouldWait;
        this.proxies = new ProxyObserver(responseFilter, log);
        proxies.Subscribe(this.proxies);
    }

    public async Task<IRequestContext> TryGetProxy()
    {
        var (ctx, lastUsage) = proxies.GetProxy();
        await Task.Delay(shouldWait(lastUsage));
        return new DefaultRequestContext(ctx.Client);
    }
}
