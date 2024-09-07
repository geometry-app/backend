using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Folleach.Properties;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations.ImpossibleList;

public class ImpossibleListExplorer(IPropertiesProvider properties) : IExplorer<ImpossibleListRequest>
{
    private readonly IPropertiesProvider properties = properties;
    private readonly HttpClient client = new();

    public async Task<ExploreResult> Explore(ExploreRequest<ImpossibleListRequest> request, ILog log)
    {
        log.Info("getting levels from impossible list");
        var response = await client.GetAsync($"https://pb.impossible-list.com/api/collections/ill/records?page={request.Properties.Page}&perPage=100&sort=position");
        if (response.StatusCode != HttpStatusCode.OK)
            return new ExploreResult(ExploreStatus.Error, message: $"response code is: {response.StatusCode}");
        var data = await response.Content.ReadAsByteArrayAsync();
        if (!await properties.AddToExistsDataAsync(data, log, request))
            return new ExploreResult(ExploreStatus.NotModified, message: "hash already exists");
        log.Info("success");
        return data.AsSuccess();
    }
}
