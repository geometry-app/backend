using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GeometryApp.Common;
using GeometryApp.Explorer.Implementations;

namespace GeometryApp.Explorer;

public static class ExploreRequestExtensions
{
    public static async Task<ExploreResult> ExecuteOn<T>(this ExploreRequest<T> request, IRequestExecutor executor) where T : IExploreRequest
    {
        return await executor.Execute(request);
    }

    private static string ToFirstLower(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        var first = value[0].ToString().ToLower();
        return $"{first}{value.Remove(0, 1)}";
    }
}
