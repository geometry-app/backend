using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer;

public class ExploreResult
{
    public ExploreStatus Status { get; set; }
    public byte[]? Value { get; }
    public string? Message { get; set; }

    public ExploreResult(ExploreStatus status, byte[]? value = null, string? message = null)
    {
        Status = status;
        Value = value;
        Message = message;
    }
}

public static class ExploreResultExtensions
{
    public static ExploreResult AsSuccess(this byte[] value)
    {
        return new ExploreResult(ExploreStatus.Success, value);
    }

    public static ILog WithResult(this ILog log, ExploreResult result)
    {
        return log
            .WithProperty("explorerStatus", result.Status.ToString())
            .WithProperty("explorerMessage", result.Message)
            .WithProperty("explorerResponseLength", result.Value?.Length ?? 0);
    }
}
