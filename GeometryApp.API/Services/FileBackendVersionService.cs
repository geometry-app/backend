using System.IO;
using System.Linq;
using GeometryApp.Common;

namespace GeometryApp.API.Services;

public class FileBackendVersionService : IBackendVersionService
{
    private readonly IAppEnvironment environment;
    private readonly string version;

    public FileBackendVersionService(string file, IAppEnvironment environment)
    {
        this.environment = environment;
        version = (File.Exists(file) ? File.ReadAllLines(file).FirstOrDefault()?.Trim() : null) ?? "not specified";
    }

    public (string version, string environment) Get()
    {
        return (version, environment.Get());
    }
}
