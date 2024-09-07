using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeometryApp.Common.Entities;
using GeometryDashAPI;
using GeometryDashAPI.Server;

namespace GeometryApp.Common;

public static class ServerResponseExtensions
{
    public static byte[] ToUniversalJson<T, TRequest>(this ServerResponse<T> response, int id, TRequest request) where T : IGameObject
    {
        return JsonSerializer.SerializeToUtf8Bytes(new GeometryDashResponse()
        {
            Id = id,
            RequestProperties = JsonSerializer.Serialize(request),
            Raw = response.GetRawOrDefault(),
            CheckDate = DateTime.UtcNow,
            StatusCode = response.GeometryDashStatusCode,
            HttpStatusCode = (int)response.HttpStatusCode
        });
    }

    public static string ToUniversalJsonString<T, TRequest>(this ServerResponse<T> response, int id, TRequest request) where T : IGameObject
    {
        return JsonSerializer.Serialize(new GeometryDashResponse()
        {
            Id = id,
            RequestProperties = JsonSerializer.Serialize(request),
            Raw = response.GetRawOrDefault(),
            CheckDate = DateTime.UtcNow,
            StatusCode = response.GeometryDashStatusCode,
            HttpStatusCode = (int)response.HttpStatusCode
        });
    }

    public static ServerResponse<T> ToServerResponse<T>(this GeometryDashResponse response) where T : IGameObject
    {
        return new ServerResponse<T>((HttpStatusCode)response.HttpStatusCode, response.Raw);
    }
}
