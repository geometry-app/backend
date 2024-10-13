using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeometryApp.Common.Filters;
using GeometryDashAPI.Server.Enums;

namespace GeometryApp.API.Services.Filters.Definitions;

public class LengthFilter : IFilter, IAutoComplete
{
    public const string Field = "metaPreview.length";

    public string Name => "length";

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Term, Name);
    }

    public IEnumerable<InternalFilter> Enrich(Filter item)
    {
        yield return new InternalFilter(Field, [Map(item.Value)], (InternalFilterOperator)item.Operator);
    }

    public Task<string[]> GetCompletionsAsync() => Task.FromResult(Enum.GetNames<LengthType>());

    private static string Map(string value)
    {
        foreach (var name in Enum.GetNames<LengthType>())
        {
            if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                return ((int)Enum.Parse<LengthType>(value, true)).ToString();
        }
        return value;
    }
}
