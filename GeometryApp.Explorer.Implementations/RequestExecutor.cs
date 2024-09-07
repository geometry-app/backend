using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations;

public class RequestExecutor : IRequestExecutor
{
    private readonly ILog instanceLog;
    private readonly IServiceProvider provider;

    public RequestExecutor(ILog log, IServiceProvider provider)
    {
        instanceLog = log.ForContext("ExploreRequestExecutor");
        this.provider = provider;
    }

    public async Task<ExploreResult> Execute<T>(ExploreRequest<T> request) where T : IExploreRequest
    {
        var log = instanceLog.WithRequest(request);
        log.Info($"Try explore request: {request.RequestId}");
        try
        {
            var explorer = provider.GetService<IExplorer<T>>();
            if (explorer == null)
            {
                log.Error($"explorer for type: '{request.Type}' not registered");
                return new ExploreResult(ExploreStatus.Error);
            }

            var result = await explorer.Explore(request, log);
            log.WithResult(result).Info($"explore proceeded. status: '{result.Status}' and message: '{result.Message}'");
            return result;
        }
        catch (Exception e)
        {
            log.Error(e, $"exception while exploring: '{request.Type}', status: {request}");
            return new ExploreResult(ExploreStatus.Error);
        }
    }
}
