using System.Text.Json.Serialization;

namespace GeometryApp.Common.Filters;

public record QueryRequest(
    [property: JsonPropertyName("t")] string? Text,
    [property: JsonPropertyName("f")] Filter[] Filters);
