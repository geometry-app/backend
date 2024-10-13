using GeometryApp.Common;
using GeometryApp.Common.Filters;
using GeometryApp.Common.Models.Elastic.Levels;
using GeometryApp.Common.Models.Front;
using GeometryApp.Repositories.Elastic;
using GeometryDashAPI.Server.Enums;
using Nest;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Services.Roulette.Generators;

public class RouletteGenerator : IRouletteGenerator
{
    private readonly ElasticApp elastic;
    private readonly ILog log;

    public RouletteGenerator(ElasticApp elastic, ILog log)
    {
        this.elastic = elastic;
        this.log = log;
    }

    public async Task<Roulette> CreateRoulette(Guid id, RouletteLevelWeights weights, PreparedRequest? request, params (string key, string value)[] properties)
    {
        var type = properties.FirstOrDefault(x => x.key == "type").value;
        var server = properties.FirstOrDefault(x => x.key == "server").value;

        var documents = type.ToLower() switch
        {
            "impossible_list" => CreateImpossibleList(),
            "challenge" => CreateChallenge(server, weights),
            "auto" => CreateAuto(server),
            "shitty" => CreateShitty(),
            "advance" => CreateAdvance(request!, weights),
            _ => CreateDefault(server)
        };
        var roulette = Create(id, await documents);
        return roulette;
    }

    private async Task<List<LevelIndexFull>> CreateAdvance(PreparedRequest request, RouletteLevelWeights weights)
    {
        var documents = new List<LevelIndexFull>();
        var ids = new List<string>();
        Exception? lastException = null;
        using (var timer = new AutoTimer(log, "roulette mining documents"))
        {
            for (var i = 0; i < 500 && documents.Count != 400; i++)
            {
                var difficulty = weights.GetRandomV2() ?? weights.GetRandomV2() ?? throw new InvalidOperationException("failed to select random difficulty");
                try
                {
                    var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(x => x
                        .Query(q => q
                            .FunctionScore(s => s
                                .Query(sq => sq.ApplyQueryRequest(
                                    request,
                                    [
                                        m => m.Match(mm => mm.Field(f => f.MetaPreview!.DifficultyIcon).Query(((int)difficulty.difficulty).ToString())),
                                        m => m.Match(mm => mm.Field(f => f.MetaPreview!.IsDemon).Query(difficulty.demon ? "true" : "false"))
                                    ],
                                    [m => m.Ids(x => x.Values(ids))])
                                )
                                .Boost(2)
                                .Functions(fu => fu.RandomScore())
                                .BoostMode(FunctionBoostMode.Multiply)
                            )
                        )
                        .Size(1)
                    );
                    if (result.ServerError != null)
                    {
                        lastException = new InvalidOperationException(result.ServerError.ToString(), result.OriginalException);
                    }

                    var hit = result.Hits.FirstOrDefault();
                    if (hit != null)
                    {
                        documents.Add(hit.Source);
                        ids.Add(hit.Id);
                    }
                    else
                    {
                        ids.Clear();
                    }
                }
                catch (Exception e)
                {
                    lastException = e;
                }
            }
        }

        if (documents.Count < 400 && lastException != null)
            throw lastException;
        if (documents.Count < 400)
            throw new InvalidOperationException("can't find 400 documents");
        return documents;
    }

    private async Task<List<LevelIndexFull>> CreateShitty()
    {
        var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(x => x
            .Query(q => q
                .FunctionScore(s => s
                    .Query(sq => sq
                        .Bool(b => b
                            .Must(m => m
                                    .Exists(fi => fi
                                        .Field("metaPreview")
                                    ), m => m
                                    .Exists(fi => fi
                                        .Field("badges.shitty.position")
                                    )
                            )
                        )
                    )
                    .Boost(2)
                    .Functions(Enumerable.Range(0, 6).Select(_ => new RandomScoreFunction()))
                    .BoostMode(FunctionBoostMode.Multiply)
                )
            )
            .Size(400)
        );
        if (result.ServerError != null)
            throw new InvalidOperationException(result.ServerError.ToString(), result.OriginalException);
        return result.Documents.ToList();
    }

    private async Task<List<LevelIndexFull>> CreateAuto(string server)
    {
        var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(x => x
            .Query(q => q
                .FunctionScore(s => s
                    .Query(sq => sq
                        .Bool(b => b
                            .Must(
                                m => m.Exists(fi => fi.Field("metaPreview")),
                                m => m.Match(fi => fi.Field("metaPreview.difficultyIcon").Query("-10")),
                                m => m.Match(field => field.Field("server").Query("geometrydash"))
                            )
                        )
                    )
                    .Boost(2)
                    .Functions(Enumerable.Range(0, 6).Select(_ => new RandomScoreFunction()))
                    .BoostMode(FunctionBoostMode.Multiply)
                )
            )
            .Size(400)
        );
        if (result.ServerError != null)
            throw new InvalidOperationException(result.ServerError.ToString(), result.OriginalException);
        return result.Documents.ToList();
    }

    private async Task<List<LevelIndexFull>> CreateImpossibleList()
    {
        var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(x => x
            .Query(q => q
                .FunctionScore(s => s
                    .Query(sq => sq
                        .Bool(b => b
                            .Must(m => m
                                    .Exists(fi => fi
                                        .Field("metaPreview")
                                    ), m => m
                                    .Exists(fi => fi
                                        .Field("badges.impossible_list.position")
                                    )
                            )
                        )
                    )
                    .Boost(2)
                    .Functions(Enumerable.Range(0, 6).Select(_ => new RandomScoreFunction()))
                    .BoostMode(FunctionBoostMode.Multiply)
                )
            )
            .Size(400)
        );
        if (result.ServerError != null)
            throw new InvalidOperationException(result.ServerError.ToString(), result.OriginalException);
        return result.Documents.ToList();
    }

    private async Task<List<LevelIndexFull>> CreateDefault(string server)
    {
        var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(x => x
            .Query(q => q
                .FunctionScore(s => s
                    .Query(sq => sq
                        .Bool(b => b
                            .Must(
                                m => m.Exists(e => e.Field(f => f.MetaPreview!.Id)),
                                m => m.Match(mm => mm.Field(f => f.MetaPreview!.IsDemon).Query("true")),
                                m => m.Match(mm => mm.Field(f => f.Server).Query(server))
                            )
                        )
                    )
                    .Boost(2)
                    .Functions(Enumerable.Range(0, 6).Select(x => new RandomScoreFunction() {
                        Filter = new QueryContainerDescriptor<LevelIndexFull>().Match(m => m.Field(w => w.MetaPreview!.DemonDifficulty == x)),
                        Weight = 1
                    }))
                    .BoostMode(FunctionBoostMode.Multiply)
                )
            )
            .Size(400)
        );
        if (result.ServerError != null)
            throw new InvalidOperationException(result.ServerError.ToString(), result.OriginalException);
        return result.Documents.ToList();
    }

    private async Task<List<LevelIndexFull>> CreateChallenge(string server, RouletteLevelWeights weights)
    {
        var documents = new List<LevelIndexFull>();
        var ids = new List<string>();
        Exception? lastException = null;
        using (var timer = new AutoTimer(log, "roulette mining documents"))
        {
            for (var i = 0; i < 500 && documents.Count != 400; i++)
            {
                var demonDifficulty = weights.GetRandom();
                try
                {
                    var result = await elastic.GetClient().SearchAsync<LevelIndexFull>(x => x
                        .Query(q => q
                            .FunctionScore(s => s
                                .Query(sq => sq
                                    .Bool(b => b
                                        .Must(
                                            m => m.Exists(e => e.Field(f => f.MetaPreview!.Id)),
                                            m => m.Match(mm => mm.Field(f => f.MetaPreview!.IsDemon).Query("true")),
                                            m => m.Match(mm => mm.Field(f => f.MetaPreview!.DemonDifficulty).Query(GetDifficulty(demonDifficulty))), 
                                            m => m.Match(mm => mm.Field(f => f.Server).Query(server)))
                                        .MustNot(m => m
                                            .Ids(x => x.Values(ids))
                                        )
                                    )
                                )
                                .Boost(2)
                                .Functions(fu => fu
                                    .RandomScore()
                                )
                                .BoostMode(FunctionBoostMode.Multiply)
                            )
                        )
                        .Size(1)
                    );
                    if (result.ServerError != null)
                    {
                        lastException = new InvalidOperationException(result.ServerError.ToString(), result.OriginalException);
                    }

                    var hit = result.Hits.FirstOrDefault();
                    if (hit != null)
                    {
                        documents.Add(hit.Source);
                        ids.Add(hit.Id);
                    }
                    else
                    {
                        ids.Clear();
                    }
                }
                catch (Exception e)
                {
                    lastException = e;
                }
            }
        }

        if (documents.Count < 400 && lastException != null)
            throw lastException;
        if (documents.Count < 400)
            throw new InvalidOperationException("can't find 400 documents");
        return documents;
    }
    
    private string GetDifficulty(DemonDifficulty demonDifficulty)
    {
        return demonDifficulty switch
        {
            DemonDifficulty.Easy => "3",
            DemonDifficulty.Medium => "4",
            DemonDifficulty.Hard => "0",
            DemonDifficulty.Insane => "5",
            DemonDifficulty.Extreme => "6",
            _ => throw new ArgumentOutOfRangeException(nameof(demonDifficulty), demonDifficulty, null)
        };
    }

    private static Roulette Create(Guid id, IEnumerable<LevelIndexFull> levels)
    {
        var list = levels.ToList();
        Stack<LevelIndexFull>? notUsed = null;
        var random = new Random();
        var primaries = new List<LevelIndexFull>(100);
        for (var i = 0; i < 100; i++)
        {
            if (i < list.Count)
                primaries.Add(list[i]);
            else
                primaries.Add(list[random.Next(list.Count)]);
        }

        LevelPreviewDto GetRandom()
        {
            if (notUsed == null || notUsed.Count == 0)
                notUsed = new Stack<LevelIndexFull>(list.Where(x => !primaries.Contains(x)).OrderBy(x => random.NextDouble() - 0.5 > 0 ? 1 : -1));
            return CreateRouletteLevel(notUsed.Count == 0 ? list[random.Next(list.Count)] : notUsed.Pop());
        }

        var entries = new List<RouletteEntry>();

        foreach (var primary in primaries)
        {
            entries.Add(new RouletteEntry()
            {
                Levels = new LevelPreviewDto[]
                {
                    CreateRouletteLevel(primary),
                    GetRandom(),
                    GetRandom(),
                    GetRandom()
                }
            });
        }

        var roulette = new Roulette()
        {
            RouletteId = id,
            Levels = entries
        };
        return roulette;
    }

    private static LevelPreviewDto CreateRouletteLevel(LevelIndexFull level)
    {
        try
        {
            return level.GetPreview();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
