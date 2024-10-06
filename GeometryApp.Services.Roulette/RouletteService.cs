using System.Text.Json;
using Folleach.Properties;
using GeometryApp.Common;
using GeometryApp.Common.Filters;
using GeometryApp.Services.Roulette.Generators;
using GeometryApp.Services.Roulette.Properties;
using GeometryApp.Services.Roulette.Repositories;
using GeometryApp.Services.Roulette.SessionGenerator;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Services.Roulette;

public class RouletteService(
    IPropertiesProvider properties,
    IRouletteGenerator generator,
    IRouletteSessionGenerator sessionGenerator,
    IRouletteRepository rouletteRepository,
    IProgressRepository progress,
    IRouletteOwnerRepository rouletteOwners,
    ILog log)
{
    private readonly IPropertiesProvider properties = properties;
    private readonly IRouletteGenerator generator = generator;
    private readonly IRouletteSessionGenerator sessionGenerator = sessionGenerator;
    private readonly IRouletteRepository rouletteRepository = rouletteRepository;
    private readonly IProgressRepository progress = progress;
    private readonly IRouletteOwnerRepository rouletteOwners = rouletteOwners;
    private readonly ILog log = log.ForContext("RouletteService");

    public async Task<RouletteSession> CreateSession(string type, string name, string server, DemonWeights weights, PreparedRequest? request, string? sessionId)
    {
        log.Info($"creating roulette with type: {type}");
        var rouletteId = Guid.NewGuid();
        sessionId = RecreateSession(sessionId);
        await rouletteOwners.Put(sessionId, owner: true, rouletteId);
        var insertRoulette = properties.Insert(RouletteProperties.Scope, rouletteId.ToString(), new RouletteProperties()
        {
            Type = type,
            Parameters = request != null ? JsonSerializer.Serialize(request) : null,
            OwnerSession = sessionId,
            Name = name,
            Id = rouletteId,
            CreateDt = DateTime.UtcNow
        });
        var insertSession = properties.Insert("roulette/sessions", sessionId, new SessionProperties()
        {
            RouletteId = rouletteId.ToString()
        });
        var roulette = generator.CreateRoulette(rouletteId, weights, request, ("type", type), ("server", server));
        await Task.WhenAll(insertRoulette, insertSession, roulette);
        insertRoulette.GetAwaiter().GetResult();
        insertSession.GetAwaiter().GetResult();
        var rouletteResult = roulette.GetAwaiter().GetResult();
        await rouletteRepository.Save(roulette.Result);
        return new RouletteSession()
        {
            SessionId = sessionId,
            Roulette = rouletteResult,
            IsStarted = true
        };
    }

    public async Task<RouletteSession?> GetSession(string sessionId, Guid rouletteId)
    {
        sessionId = RecreateSession(sessionId);
        var rouletteTask = rouletteRepository.Get(rouletteId);
        var stateTask = progress.Get(rouletteId, sessionId);
        var propsTask = properties.Get<RouletteProperties>(RouletteProperties.Scope, rouletteId.ToString());
        await Task.WhenAll(rouletteTask, stateTask, propsTask);
        var roulette = rouletteTask.GetAwaiter().GetResult();
        var state = stateTask.GetAwaiter().GetResult().ToArray();
        var props = propsTask.GetAwaiter().GetResult();
        if (props == null || !IsAllow(props, sessionId))
            return null;
        roulette.Name = props.Name ?? "unnamed";
        roulette.IsPublished = props.IsPublished;

        return new RouletteSession()
        {
            Roulette = roulette,
            SessionId = sessionId,
            Progress = state,
            IsStarted = state.Any() || props.OwnerSession == sessionId
        };
    }

    private string RecreateSession(string? sessionId)
    {
        if (sessionId == null || sessionId.Length < 10)
            return sessionGenerator.GenerateSessionId();
        return sessionId;
    }

    public async Task<bool> SetProgress(string sessionId, Guid rouletteId, int sequenceNumber, int current)
    {
        var (roulette, props) = await MultiTask.ExecuteAll(
            rouletteRepository.Get(rouletteId),
            properties.Get<RouletteProperties>(RouletteProperties.Scope, rouletteId.ToString()));
        if (props == null || !IsAllow(props, sessionId) || roulette.Levels == null)
            return false;
        if (sequenceNumber >= roulette.Levels.Count)
            return false;
        var levels = roulette.Levels[sequenceNumber];
        var level = levels.Levels!.FirstOrDefault();
        if (level == null)
            return false;
        await progress.Set(rouletteId, sessionId, sequenceNumber, level.Id, current);
        return true;
    }

    public async Task<IEnumerable<RouletteSessionPreview>> GetSessions(string sessionId)
    {
        sessionId = RecreateSession(sessionId);
        var all = await rouletteOwners.GetAll(sessionId);
        var tasks = all
            .Select(x => properties.Get<RouletteProperties>(RouletteProperties.Scope, x.RouletteId.ToString()))
            .ToArray();
        await Task.WhenAll(tasks);
        return tasks.Select(x => new RouletteSessionPreview()
        {
            Id = x.Result.Id,
            Name = x.Result.Name ?? "unnamed",
            Type = x.Result.Type,
            Owner = x.Result.OwnerSession == sessionId,
            IsPublic = x.Result.IsPublished,
            CreateDt = x.Result.CreateDt
        });
    }

    public async Task<bool> PublishRoulette(Guid rouletteId, string sessionId)
    {
        var roulette = await properties.Get<RouletteProperties>(RouletteProperties.Scope, rouletteId.ToString());
        if (roulette == null || !IsOwner(roulette, sessionId))
            return false;
        roulette.IsPublished = true;
        await properties.Insert(RouletteProperties.Scope, rouletteId.ToString(), roulette);
        return true;
    }

    public async Task<bool> AddPublicRoulette(Guid rouletteId, string sessionId)
    {
        var props = await properties.Get<RouletteProperties>(RouletteProperties.Scope, rouletteId.ToString());
        if (props == null || !IsAllow(props, sessionId))
            return false;
        await rouletteOwners.Put(sessionId, owner: false, rouletteId);
        return true;
    }

    private bool IsOwner(RouletteProperties props, string sessionId)
    {
        return props.OwnerSession == sessionId;
    }

    private bool IsAllow(RouletteProperties props, string sessionId)
    {
        return IsOwner(props, sessionId) || props.IsPublished;
    }
}
