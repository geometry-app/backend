using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Folleach.Properties;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations.PointCreate;

public class PointCreateExplorer : IExplorer<PointCreateRequest>
{
    private readonly IPropertiesProvider properties;
    private readonly HttpClient client = new();

    public PointCreateExplorer(IPropertiesProvider properties)
    {
        this.properties = properties;
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<ExploreResult> Explore(ExploreRequest<PointCreateRequest> request, ILog log)
    {
        log.Info("getting demons from pointcreate");
        var response = await client.GetAsync("https://pointercrate.com/api/v2/demons/listed?limit=100");
        if (response.StatusCode != HttpStatusCode.OK)
            return new ExploreResult(ExploreStatus.Error, message: $"response code is: {response.StatusCode}");
        var data = await response.Content.ReadAsByteArrayAsync();
        if (!await properties.AddToExistsDataAsync(data, log, request))
            return new ExploreResult(ExploreStatus.NotModified, message: "hash already exists");
        log.Info("success");
        return data.AsSuccess();
    }
}

public class ExplorerProp
{
    public string? LastAccess { get; init; }
    public string? LastHash { get; init; }
}
