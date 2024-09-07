using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GeometryApp.API.Controllers.Roulette.Add;
using GeometryApp.API.Controllers.Roulette.Balance;
using GeometryApp.API.Controllers.Roulette.Create;
using GeometryApp.API.Controllers.Roulette.Progress;
using GeometryApp.API.Controllers.Roulette.Publish;
using GeometryApp.Common.Models.Elastic.Levels;
using GeometryApp.Explorer;
using GeometryApp.Repositories.Elastic;
using GeometryApp.Services.Roulette;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace GeometryApp.API.Controllers.Roulette;

[ApiController]
[Route("api/roulette")]
public class RouletteController : ControllerBase
{
    private readonly RouletteService service;
    private readonly ElasticApp elastic;

    public RouletteController(RouletteService service, ElasticApp elastic)
    {
        this.service = service;
        this.elastic = elastic;
    }

    [HttpGet]
    public async Task<RouletteSession> GetSession([FromQuery] GetSessionRequest request, [FromHeader] string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
            return null;
        return await service.GetSession(sessionId, request.RouletteId);
    }

    /// <summary>
    /// Get all started roulettes
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("sessions")]
    public async Task<IEnumerable<RouletteSessionPreview>> GetSessions([FromHeader] string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
            return null;
        return await service.GetSessions(sessionId);
    }

    private readonly Regex RouletteNameValidator = new Regex(@"^[a-zA-Z0-9 -]+$");

    private readonly HashSet<string> possibleTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "default", "challenge", "impossible_list", "auto", "shitty"
    };

    [HttpPost]
    public async Task<RouletteSession> CreateSession([FromBody] CreateRouletteSessionRequest request, [FromHeader] string sessionId)
    {
        if (!possibleTypes.Contains(request.Type))
            return null;
        var server = request.Server == Resources.GeometryDashServer || request.Server == Resources.GdpsEditorServer
            ? request.Server
            : Resources.GeometryDashServer;

        if (!RouletteNameValidator.IsMatch(request.Name))
        {
            HttpContext.Response.StatusCode = 400;
            return null;
        };
        var rouletteSession = await service.CreateSession(request.Type, request.Name, server, request.Weights, sessionId);
        return rouletteSession;
    }

    [HttpPost]
    [Route("progress")]
    public async Task<SetProgressResponse> SetProgress([FromBody] SetProgressRequest request)
    {
        var result = await service.SetProgress(request.SessionId, request.RouletteId, request.SequenceNumber, request.Progress);
        return new SetProgressResponse()
        {
            Success = result
        };
    }

    [HttpPost]
    [Route("publish")]
    public async Task<PublishResponse> PublishRoulette([FromBody] PublishRequest request, [FromHeader] string sessionId)
    {
        var result = await service.PublishRoulette(request.RouletteId, sessionId);
        return new PublishResponse()
        {
            IsPublished = result
        };
    }

    [HttpPost]
    [Route("add")]
    public async Task<AddRouletteResponse> AddRoulette([FromBody] AddRouletteRequest request, [FromHeader] string sessionId)
    {
        var result = await service.AddPublicRoulette(request.RouletteId, sessionId);
        return new AddRouletteResponse()
        {
            Added = result
        };
    }

    [HttpGet]
    [Route("balance")]
    public async Task<BalanceResponse> GetBalance()
    {
        var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(s => s
            .Size(0)
            .Aggregations(a => a
                .Terms("counts", t => t
                    .Field(f => f.MetaPreview.DemonDifficulty)
                )
            )
        );
        var aggregation = (BucketAggregate)result.Aggregations.FirstOrDefault(x => x.Key == "counts").Value;
        var dict = aggregation.Items.Cast<KeyedBucket<object>>().ToDictionary(x => (long)x.Key, x => x.DocCount);
        var sum = (float)(dict.Sum(x => x.Value) ?? 0);
        var max = GetChance(dict, dict.FirstOrDefault(x => x.Value == dict.Max(m => m.Value)).Key, sum, 1);
        var multiply = 1 / max;
        return new BalanceResponse()
        {
            Weights = new DemonWeights()
            {
                EasyDemon = GetChance(dict, 3, sum, multiply),
                MediumDemon = GetChance(dict, 4, sum, multiply),
                HardDemon = GetChance(dict, 0, sum, multiply),
                InsaneDemon = GetChance(dict, 5, sum, multiply),
                ExtremeDemon = GetChance(dict, 6, sum, multiply),
            }
        };
    }

    private static float GetChance(Dictionary<long, long?> dict, long key, float sum, float multiply)
    {
        if (sum == 0)
            return 0;
        return dict.TryGetValue(key, out var value) && value != null ? Math.Min(1, MathF.Round(value.Value / sum * multiply, 2)) : 0f;
    }

    // private async Task<RouletteSession> CreateTypedSession(CreateRouletteSessionRequest request)
    // {
    //     if (!string.IsNullOrEmpty(request.Type))
    //         return await service.CreateSession(request.Type, request.Name, );
    //
    //     throw new ArgumentException();
    // }
}
