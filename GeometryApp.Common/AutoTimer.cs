using System;
using System.Diagnostics;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Common;

public class AutoTimer : IDisposable
{
    private readonly ILog log;
    private readonly string? prefix;
    private readonly Stopwatch time;

    public AutoTimer(ILog log, string? prefix = null)
    {
        this.log = log;
        this.prefix = prefix;
        time = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        time.Stop();
        log
            .WithProperty("elapsedMs", time.ElapsedMilliseconds)
            .Info($"{(prefix != null ? $"{prefix} " : "")}elapsed: {time.Elapsed}");
    }
}
