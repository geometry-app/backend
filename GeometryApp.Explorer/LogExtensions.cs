using GeometryApp.Common;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer;

public static class LogExtensions
{
    public static ILog WithRequest<T>(this ILog log, ExploreRequest<T> request) where T : IExploreRequest
    {
        return log
            .ForContext(request.RequestId.ToString())
            .WithProperty("dataRequestId", request.RequestId)
            .WithProperty("dataType", request.Type);
    }

    public static ILog WithDataItem(this ILog log, DataItem item)
    {
        return log
            .ForContext(item.RequestId.ToString())
            .WithProperty("dataRequestId", item.RequestId)
            .WithProperty("dataType", item.Type);
    }
}
