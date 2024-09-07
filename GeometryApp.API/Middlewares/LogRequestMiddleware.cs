using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vostok.Logging.Abstractions;

namespace GeometryApp.API.Middlewares;

public class LogRequestMiddleware : IMiddleware
{
    private readonly ILog log;

    public LogRequestMiddleware(ILog log)
    {
        this.log = log.ForContext(nameof(LogRequestMiddleware));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        log.Info($"request has come: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
        await next(context);
    }
}
