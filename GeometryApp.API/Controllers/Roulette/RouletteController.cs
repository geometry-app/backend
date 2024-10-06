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
using GeometryApp.API.Controllers.Search;
using GeometryApp.API.Services;
using GeometryApp.Common.Filters;
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
    private readonly FiltersService filters;

    public RouletteController(RouletteService service, ElasticApp elastic, FiltersService filters)
    {
        this.service = service;
        this.elastic = elastic;
        this.filters = filters;
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
        "default", "challenge", "impossible_list", "auto", "shitty", "advance"
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
        if (request.Type.Equals("Advance", StringComparison.OrdinalIgnoreCase) && request.Request == null)
            return null;
        var prepared = request.Request != null ? new PreparedRequest(SearchHelper.RemoveIllegalCharacter(request.Request.Text), filters.Enrich(request.Request.Filters).ToArray()) : null;
        var rouletteSession = await service.CreateSession(request.Type, request.Name, server, request.Weights, prepared, sessionId);
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
            Weights = new RouletteLevelWeights()
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

    [HttpGet]
    [Route("v2/balance")]
    public async Task<BalanceResponse> GetBalanceV2([FromQuery] string query)
    {
        var prepared = SearchHelper.ParseAndPrepare(query, filters);
        if (prepared == null)
            return new BalanceResponse();
        var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(s => s
            .Size(0)
            .Query(q => q.ApplyQueryRequest(prepared))
            .Aggregations(a => a
                .Filter("demons_count", f => f
                    .Filter(ff => ff.Term(t => t.Field(f => f.MetaPreview.IsDemon).Value(true)))
                    .Aggregations(af => af
                        .Terms("demons_count_inside", t => t
                            .Field(f => f.MetaPreview.DifficultyIcon)
                        )
                    )
                )
                .Filter("others_count", f => f
                    .Filter(ff => ff.Term(t => t.Field(f => f.MetaPreview.IsDemon).Value(false)))
                    .Aggregations(af => af
                        .Terms("others_count_inside", t => t
                            .Field(f => f.MetaPreview.DifficultyIcon)
                        )
                    )
                )
            )
        );
        var demonsSingle = (SingleBucketAggregate)result.Aggregations.FirstOrDefault(x => x.Key == "demons_count").Value;
        var othersSingle = (SingleBucketAggregate)result.Aggregations.FirstOrDefault(x => x.Key == "others_count").Value;
        var demonsAggregation = ((BucketAggregate)demonsSingle.First().Value)
            .Items
            .Cast<KeyedBucket<object>>()
            .Select(x => (type: "d", item: x));
        var othersAggregation = ((BucketAggregate)othersSingle.First().Value)
            .Items
            .Cast<KeyedBucket<object>>()
            .Select(x => (type: "o", item: x));
        var dict = demonsAggregation
            .Concat(othersAggregation)
            .ToDictionary(x => (x.type, (long)x.item.Key), x => x.item.DocCount);
        var sum = (float)(dict.Sum(x => x.Value) ?? 0);
        var max = GetChanceV2(dict, dict.FirstOrDefault(x => x.Value == dict.Max(m => m.Value)).Key, sum, 1);
        var multiply = 1 / max;
        return new BalanceResponse()
        {
            Weights = new RouletteLevelWeights()
            {
                Auto = GetChanceV2(dict, ("o", -10), sum, multiply),
                Undef = GetChanceV2(dict, ("o", 0), sum, multiply),
                Easy = GetChanceV2(dict, ("o", 10), sum, multiply),
                Normal = GetChanceV2(dict, ("o", 20), sum, multiply),
                Hard = GetChanceV2(dict, ("o", 30), sum, multiply),
                Harder = GetChanceV2(dict, ("o", 40), sum, multiply),
                Insane = GetChanceV2(dict, ("o", 50), sum, multiply),
                EasyDemon = GetChanceV2(dict, ("d", 10), sum, multiply),
                MediumDemon = GetChanceV2(dict, ("d", 20), sum, multiply),
                HardDemon = GetChanceV2(dict, ("d", 30), sum, multiply),
                InsaneDemon = GetChanceV2(dict, ("d", 40), sum, multiply),
                ExtremeDemon = GetChanceV2(dict, ("d", 50), sum, multiply),
            }
        };
    }

    private static float GetChanceV2(Dictionary<(string type, long key), long?> dict, (string type, long key) key, float sum, float multiply)
    {
        if (sum == 0)
            return 0;
        return dict.TryGetValue(key, out var value) && value != null
            ? Math.Min(1, MathF.Round(value.Value / sum * multiply, 2))
            : 0f;
    }

    // private async Task<RouletteSession> CreateTypedSession(CreateRouletteSessionRequest request)
    // {
    //     if (!string.IsNullOrEmpty(request.Type))
    //         return await service.CreateSession(request.Type, request.Name, );
    //
    //     throw new ArgumentException();
    // }
}
