using System;
using System.Text.Json;
using GeometryApp.Common.Entities;

namespace GeometryApp.Common;

public class DataItem
{
    public string Id { get; set; }
    public Guid RequestId { get; set; }
    public DateTime CreateDt { get; set; }
    public string Type { get; set; }
    public byte[]? Response { get; set; }
    public string? ContentText { get; set; }

    public DataItem(Guid requestId, string type, DateTime createDt, byte[] response)
    {
        RequestId = requestId;
        Type = type;
        CreateDt = createDt;
        Response = response;
    }

    public DataItem(Guid requestId, string type, DateTime createDt, string response)
    {
        RequestId = requestId;
        Type = type;
        CreateDt = createDt;
        ContentText = response;
    }

    public DataItem()
    {
    }

    public GeometryDashResponse? GetResponse()
    {
        if (Response != null)
            return JsonSerializer.Deserialize<GeometryDashResponse>(Response);
        if (ContentText != null)
            return JsonSerializer.Deserialize<GeometryDashResponse>(ContentText);
        return null;
    }
}
