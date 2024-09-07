using System;
using System.Runtime.Serialization;

namespace GeometryApp.Common.Entities;

[DataContract]
public class GeometryDashResponse
{
    [DataMember(Name = "id")] public int Id { get; set; }
    [DataMember(Name = "requestProperties")] public string? RequestProperties { get; set; }
    [DataMember(Name = "checkDate")] public DateTime CheckDate { get; set; }
    [DataMember(Name = "statusCode")] public int StatusCode { get; set; }
    [DataMember(Name = "httpStatusCode")] public int HttpStatusCode { get; set; }
    [DataMember(Name = "raw")] public string? Raw { get; set; }
}
