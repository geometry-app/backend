using System;
using GeometryDashAPI.Server;

namespace GeometryApp.Proxy;

public class ProxyContext
{
    public GameClient Client { get; set; }
    public string Proxy { get; set; }
    public DateTime LastUsage { get; set; }
}
