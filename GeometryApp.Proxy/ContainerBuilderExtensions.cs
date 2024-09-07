using System;
using System.Security.Cryptography;
using GeometryApp.Proxy.Local;
using Microsoft.Extensions.DependencyInjection;

namespace GeometryApp.Proxy;

public static class ContainerBuilderExtensions
{
    public static IServiceCollection AddLocalProxy(this IServiceCollection services)
    {
        services.AddSingleton<IProxyService>(new LocalProxyService(TimeSpan.FromSeconds(60 + RandomNumberGenerator.GetInt32(0, 60))));
        return services;
    }
}
