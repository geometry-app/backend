using System.Collections.Generic;
using System.Linq;
using GeometryApp.API.Services.Filters;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services;

public class FiltersService
{
    private readonly Dictionary<string, IFilter> filters;
    public FilterDefinition[] Definitions { get; }

    public FiltersService(IEnumerable<IFilter> filters)
    {
        this.filters = filters.ToDictionary(x => x.Name);
        Definitions = this.filters.Select(x => x.Value.GetDefinition()).ToArray();
    }

    public IEnumerable<InternalFilter> Enrich(Filter[] request)
    {
        foreach (var item in request)
        {
            if (string.IsNullOrEmpty(item.Name))
                continue;
            if (filters.TryGetValue(item.Name, out var filter))
            {
                var value = filter is IValueMapper mapper ? mapper.Map(item.Value) : item.Value;
                var @operator = filter is ICustomOperator customOperator ? customOperator.Map(item.Operator) : (InternalFilterOperator)item.Operator;
                yield return new InternalFilter(filter.Field, [value], @operator);
            }
        }
    }
}
