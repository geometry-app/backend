using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeometryApp.Explorer.Implementations.Account;
using GeometryApp.Explorer.Implementations.ImpossibleList;
using GeometryApp.Explorer.Implementations.Level;
using GeometryApp.Explorer.Implementations.PointCreate;
using Microsoft.Extensions.DependencyInjection;

namespace GeometryApp.Explorer.Implementations;

public static class ExplorerConfigurator
{
    public static IServiceCollection AddExplorers(this IServiceCollection services)
    {
        var dict = new Dictionary<Type, object>();
        var byRequestType = new Dictionary<string, Type>();
        foreach (var explorer in GetExplorers())
        {
            var interfaces = explorer.GetInterfaces();
            if (interfaces.Length != 1)
                throw new InvalidOperationException("todo: implement selecting interface of explorer");
            var explorerInterface = interfaces[0];
            var requestType = explorerInterface.GetGenericArguments().FirstOrDefault();
            services.AddSingleton(explorerInterface, explorer);

            dict.Add(requestType!, services.BuildServiceProvider().GetService(explorerInterface)!);
            var typeAttribute = (RequestTypeAttribute)requestType!.GetCustomAttribute(typeof(RequestTypeAttribute))!;
            if (typeAttribute != null)
                byRequestType.Add(typeAttribute.Type, requestType!);
        }

        services.AddSingleton(new ExplorerServices(dict, byRequestType));
        return services;
    }

    private static IEnumerable<Type> GetExplorers()
    {
        yield return typeof(LevelExplorer);
        yield return typeof(LevelSearchExplorer);
        yield return typeof(PointCreateExplorer);
        yield return typeof(AccountExplorer);
        yield return typeof(ImpossibleListExplorer);
    }
}
