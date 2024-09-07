using System;
using GeometryApp.Common;

namespace GeometryApp.Explorer.Implementations;

public static class DataItemExtensions
{
    private static readonly DataIdManager dataId = new();

    public static DataItem ToDataItem<T>(this ExploreResult result, ExploreRequest<T> request) where T : IExploreRequest
    {
        return new DataItem()
        {
            Id = dataId.GetId(request),
            RequestId = request.RequestId,
            Response = result.Value!,
            CreateDt = DateTime.UtcNow,
            Type = request.Type
        };
    }
}
