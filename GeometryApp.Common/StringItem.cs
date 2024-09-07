using System.Runtime.Serialization;

namespace GeometryApp.Common;

[DataContract]
public class StringItem
{
    [DataMember] public string Value { get; set; } = null!;
    [DataMember] public bool Highlighted { get; set; }

    public override string ToString()
    {
        return $"{(Highlighted ? "**" : "")}{Value}{(Highlighted ? "**" : "")}";
    }
}
