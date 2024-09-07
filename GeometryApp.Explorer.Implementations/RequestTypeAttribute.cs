using System;

namespace GeometryApp.Explorer.Implementations;

public class RequestTypeAttribute(string type) : Attribute
{
    public string Type { get; } = type;
}
