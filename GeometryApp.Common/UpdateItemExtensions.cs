using System.Text.Json;

namespace GeometryApp.Common;

public static class UpdateItemExtensions
{
    public static T? GetValue<T>(this DataItem item) where T : class
    {
        if (item.Response == null)
            return null;
        return JsonSerializer.Deserialize<T>(item.Response);
    }
}
