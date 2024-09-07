using System.Text.Json.Serialization;

namespace GeometryApp.Common.Filters;

public record Filter(
    [property: JsonPropertyName("n")] string Name,
    [property: JsonPropertyName("v")] string Value,
    [property: JsonPropertyName("o")] FilterOperator Operator);
