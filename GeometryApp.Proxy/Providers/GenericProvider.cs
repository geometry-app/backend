using System;
using System.Collections.Generic;

namespace GeometryApp.Proxy.Providers;

public class ProxyProvider : IObservable<string>
{
    private readonly List<string> proxies = new();

    public IDisposable Subscribe(IObserver<string> observer)
    {
        foreach (var proxy in proxies)
            observer.OnNext(proxy);
        return null;
    }

    public void Add(string proxy)
    {
        proxies.Add(proxy);
    }
}
