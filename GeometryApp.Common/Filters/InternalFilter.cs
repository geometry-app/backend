namespace GeometryApp.Common.Filters;

public record InternalFilter(string Field, string[] Values, InternalFilterOperator Operator);
