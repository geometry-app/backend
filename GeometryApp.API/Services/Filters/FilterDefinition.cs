using System;

namespace GeometryApp.API.Services.Filters;

public enum FilterType { Term, Number }

public record FilterDefinition(FilterType Type, string Name);
