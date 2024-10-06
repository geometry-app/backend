using GeometryApp.Common.Filters;

namespace GeometryApp.API.Services.Filters;

public interface ICustomOperator
{
    InternalFilterOperator Map(FilterOperator filterOperator);
}
