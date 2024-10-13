using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeometryApp.API.Controllers.Search.Autocomplete;
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
                foreach (var next in filter.Enrich(item))
                    yield return next;
            }
        }
    }

    public async Task<AutocompleteResult> GetCompletionsAsync(string name, string request)
    {
        if (!filters.TryGetValue(name, out var filter) || filter is not IAutoComplete completable)
            return new AutocompleteResult(string.Empty, []);
        var additionals = await completable.GetCompletionsAsync();
        var filtered = additionals
            .Where(x => x.StartsWith(request, System.StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Substring(request.Length))
            .ToArray();
        return new AutocompleteResult(request, filtered);
    }
}
