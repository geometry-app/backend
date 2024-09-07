using System;
using GeometryApp.Common;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer;

public class RequestFactory : IRequestFactory
{
    private readonly ILog log;
    private readonly ServiceInfo service;

    public RequestFactory(ILog log, ServiceInfo service)
    {
        this.log = log.ForContext("ExoloreRequestFactory");
        this.service = service;
    }

    public ExploreRequest<T> Create<T>(T parameters) where T : IExploreRequest
    {
        var request = new ExploreRequest<T>(parameters.Type, DateTime.UtcNow, parameters);
        log.WithRequest(request).Info($"spawn task '{request.RequestId}' by {service.Service}");
        return request;
    }
}
