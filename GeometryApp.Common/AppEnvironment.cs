using System;

namespace GeometryApp.Common;

public class AppEnvironment : IAppEnvironment
{
    public const string NoneValue = "none";
    public const string TestValue = "test";
    public const string StageValue = "stage";
    public const string ProductionValue = "production";
    public const string EnvName = "GEOMETRYAPP_ENV";

    private string Env;

    public AppEnvironment()
    {
        Env = Environment.GetEnvironmentVariable(EnvName)?.ToLower() ?? NoneValue;
    }

    public bool IsSet => !string.IsNullOrEmpty(Env) && Env != NoneValue;

    public bool IsTest => Env == TestValue;
    public bool IsStage => Env == StageValue;
    public bool IsProduction => Env == ProductionValue;

    public string Get() => Env;
}
