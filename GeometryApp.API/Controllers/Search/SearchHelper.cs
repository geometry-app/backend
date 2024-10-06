using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using GeometryApp.API.Services;
using GeometryApp.Common.Filters;

namespace GeometryApp.API.Controllers.Search;

public static class SearchHelper
{
    private static readonly Regex IllegalCharacter = new Regex(@"[^a-zA-Z0-9 ""]", RegexOptions.Compiled);

    internal static PreparedRequest? ParseAndPrepare(string? query, FiltersService filters)
    {
        try
        {
            if (query == null)
                return new PreparedRequest(null, []);
            var qdata = Encoding.UTF8.GetString(Convert.FromBase64String(query));
            var json = JsonSerializer.Deserialize<QueryRequest>(qdata, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (json == null)
                return null;
            var prepared = new PreparedRequest(RemoveIllegalCharacter(json.Text), filters.Enrich(json.Filters).ToArray());
            return prepared;
        }
        catch (Exception)
        {
            return null;
        }
    }

    internal static string? RemoveIllegalCharacter(string? query)
    {
        return query == null ? null : IllegalCharacter.Replace(query, string.Empty);
    }
}
