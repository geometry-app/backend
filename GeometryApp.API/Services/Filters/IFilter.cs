using System.Collections.Generic;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services.Filters;

public interface IFilter
{
    public string Name { get; }
    FilterDefinition GetDefinition();
    IEnumerable<InternalFilter> Enrich(Filter item);
}
