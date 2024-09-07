using System;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Folleach.Properties;
using GeometryApp.Explorer.Implementations.PointCreate;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Explorer.Implementations;

public static class PropertiesProviderExtensions
{
    private static readonly HashAlgorithm algorithm = SHA256.Create();

    internal static async Task<bool> AddToExistsDataAsync<T>(this IPropertiesProvider provider, byte[] data, ILog log, ExploreRequest<T> request)
        where T : IExploreRequest
    {
        var hash = Convert.ToBase64String(algorithm.ComputeHash(data));
        log.Info($"received data with hash: {hash}");
        var id = JsonSerializer.Serialize(request.Properties, request.Properties.GetType());
        var props = await provider.Get<ExplorerProp>(request.Type, id);
        if (props.LastHash == hash)
            return false;
        await provider.Insert(request.Type, id, new ExplorerProp()
        {
            LastAccess = DateTime.UtcNow.ToString("O"),
            LastHash = hash
        });
        return true;
    }
}
