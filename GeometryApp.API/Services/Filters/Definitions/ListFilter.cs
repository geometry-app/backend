using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services.Filters.Definitions;

public class ListFilter : IFilter, IAutoComplete
{
    public const string Field = "badges";

    public string Name => "list";

    private static readonly Dictionary<string, string> aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["pointcreate"] = "pointcreate_hardest",
        ["points"] = "pointcreate_hardest",
        ["impossible"] = "impossible_list",
        ["shitty"] = "shitty"
    };

    public FilterDefinition GetDefinition() => new(FilterType.Term, Name);

    public IEnumerable<InternalFilter> Enrich(Filter item)
    {
        yield return new InternalFilter(Field, [Map(item.Value)], Map(item.Operator));
    }

    public Task<string[]> GetCompletionsAsync() => Task.FromResult(aliases.DistinctBy(x => x.Value).Select(x => x.Key).ToArray());

    private static InternalFilterOperator Map(FilterOperator filterOperator)
    {
        return InternalFilterOperator.Exists | ((InternalFilterOperator)filterOperator & (InternalFilterOperator)FilterOperator.Not);
    }

    private static string Map(string value)
    {
        if (aliases.TryGetValue(value, out var anchor))
            return anchor;
        return value;
    }
}
