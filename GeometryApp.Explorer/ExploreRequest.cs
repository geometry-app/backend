using System;

namespace GeometryApp.Explorer;

public class ExploreRequest<T> where T : IExploreRequest
{
    public Guid RequestId { get; set; }
    public DateTime CreateAt { get; set; }
    public string Type { get; set; }
    public T Properties { get; set; }

    internal ExploreRequest(string type, DateTime createAt, T properties)
    {
        RequestId = Guid.NewGuid();
        CreateAt = createAt;
        Type = type;
        Properties = properties;
    }
}
