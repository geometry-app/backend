using System.Collections.Generic;
using System.Linq;
using GeometryApp.API.Services.Filters;
using GeometryApp.Common.Filters;
using GeometryApp.Repositories.Elastic;

namespace GeometryApp.API.Services;

public class FiltersService
{
    private readonly ElasticApp elastic;
    private readonly Dictionary<string, IFilter> filters;
    public FilterDefinition[] Definitions { get; }

    public FiltersService(ElasticApp elastic, IEnumerable<IFilter> filters)
    {
        this.elastic = elastic;
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
                yield return new InternalFilter(filter.Field, value, item.Operator);
            }
        }
    }
}
