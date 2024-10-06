using System;
using System.Collections.Generic;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services.Filters.Definitions;

public class ListFilter : IFilter, ICustomOperator, IValueMapper
{
    public string Name => "list";

    public string Field => "badges";

    private static readonly Dictionary<string, string> aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["pointcreate"] = "pointcreate_hardest",
        ["points"] = "pointcreate_hardest",
        ["impossible"] = "impossible_list",
    };

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Term, Name);
    }

    public InternalFilterOperator Map(FilterOperator filterOperator)
    {
        return InternalFilterOperator.Exists;
    }

    public string Map(string value)
    {
        if (aliases.TryGetValue(value, out var anchor))
            return anchor;
        return value;
    }
}
