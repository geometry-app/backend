using Vostok.Logging.Abstractions;

namespace GeometryApp.Common;

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }

    public Result(T? value, string? error)
    {
        Value = value;
        Error = error;
    }
}

public static class ResultExtensions
{
    public static Result<T> AsSuccess<T>(this T value) => new(value, null);
    public static Result<T> AsError<T>(this string error) => new(default, error);

    public static Result<T> AsError<T>(this string error, ILog log)
    {
        log.Info($"error: {error}");
        return new(default, error);
    }

    public static Result<T> LogError<T>(this Result<T> result, ILog log)
    {
        if (result.Error != null)
            log.Error(result.Error);
        return result;
    }
}
