using System.Text.Json.Serialization;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Controllers.Search.Filters;

public record QueryRequest(
    [property: JsonPropertyName("t")] string Text,
    [property: JsonPropertyName("f")] Filter[] Filters);
