using System.Threading.Tasks;
using GeometryApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeometryApp.API.Controllers.Infra;

[ApiController]
[Route("api/version")]
public class BackendVersionController
{
    private readonly IBackendVersionService version;

    public BackendVersionController(IBackendVersionService version)
    {
        this.version = version;
    }

    [HttpGet]
    public Task<BackendVersionResponse> Get()
    {
        var (ver, env) = version.Get();
        return Task.FromResult(new BackendVersionResponse()
        {
            Environment = env,
            Version = ver
        });
    }
}
