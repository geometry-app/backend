using System.Runtime.Serialization;
using System.Text;

namespace GeometryApp.Common;

[DataContract]
public class HighlightString
{
    [DataMember(Name = "items")] public StringItem[]? Items { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        if (Items == null)
            return string.Empty;
        foreach (var item in Items)
            builder.Append(item);
        return builder.ToString();
    }
}
