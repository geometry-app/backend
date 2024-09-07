using System;
using System.Collections.Generic;

namespace GeometryApp.Explorer.Implementations;

public class ExplorerServices 
{
    private readonly Dictionary<Type, object> explorersByRequestType;
    private readonly Dictionary<string, Type> requestsByType;

    public ExplorerServices(Dictionary<Type, object> explorersByRequestType, Dictionary<string, Type> requestsByType)
    {
        this.explorersByRequestType = explorersByRequestType;
        this.requestsByType = requestsByType;
    }

    public IExplorer<T>? Get<T>() where T : IExploreRequest
    {
        return explorersByRequestType.TryGetValue(typeof(T), out var explorer) ? (IExplorer<T>)explorer : null;
    }

    public Type GetType(string requestType)
    {
        if (!requestsByType.TryGetValue(requestType, out var type))
            throw new InvalidOperationException($"request type: '{requestType}' not found");
        return type;
    }
}
