using System.Text.Json;

namespace GeometryApp.Explorer;

public class DataIdManager
{
    public string GetId<T>(ExploreRequest<T> request) where T : IExploreRequest
    {
        return JsonSerializer.Serialize(request.Properties);
    }
}
