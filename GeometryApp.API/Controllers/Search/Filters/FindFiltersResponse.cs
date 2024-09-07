using System;
using GeometryApp.API.Services.Filters;

namespace GeometryApp.API.Controllers.Search.Filters;

public record FindFiltersResponse(FilterDefinition[] Filter);
