using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GeometryApp.API.Controllers.Test;

[ApiController]
[Route("api")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("test")]
    public async Task<string> Get()
    {
        return $"hello. i am {Dns.GetHostName()}, you are: {HttpContext.Connection.RemoteIpAddress}";
    }
}
