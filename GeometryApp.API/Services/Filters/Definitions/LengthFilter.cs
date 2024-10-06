using System;
using GeometryDashAPI.Server.Enums;

namespace GeometryApp.API.Services.Filters.Definitions;

public class LengthFilter : IFilter, IValueMapper
{
    public string Name => "length";

    public string Field => "metaPreview.length";

    public FilterDefinition GetDefinition()
    {
        return new FilterDefinition(FilterType.Term, Name);
    }

    public string Map(string value)
    {
        foreach (var name in Enum.GetNames<LengthType>())
        {
            if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                return ((int)Enum.Parse<LengthType>(value, true)).ToString();
        }
        return value;
    }
}
