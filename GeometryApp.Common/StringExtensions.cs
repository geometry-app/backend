using System;
using System.Collections.Generic;

namespace GeometryApp.Common;

public static class StringExtensions
{
    public static HighlightString? ToHighlightString(this string? value, string open = "<em>", string close = "</em>")
    {
        if (value == null)
            return null;
        try
        {
            var result = new List<StringItem>(4);
            var index = 0;
            while (index < value.Length)
            {
                var openIndex = value.IndexOf(open, index, StringComparison.Ordinal);
                if (openIndex == -1)
                    openIndex = index;
                var closeIndex = value.IndexOf(close, index, StringComparison.Ordinal);
                if (index != openIndex)
                    result.Add(new StringItem() { Value = value.Substring(index, openIndex - index) });
                if (closeIndex != -1)
                {
                    result.Add(new StringItem()
                    {
                        Value = value.Substring(openIndex + open.Length, closeIndex - openIndex - open.Length),
                        Highlighted = true
                    });
                }
                else
                {
                    result.Add(new StringItem() { Value = value.Substring(openIndex, value.Length - openIndex) });
                    break;
                }
                index = closeIndex + close.Length;
            }

            return new HighlightString()
            {
                Items = result.ToArray()
            };
        }
        catch
        {
            return new HighlightString() { Items = [new StringItem() { Value = value }] };
        }
    }
}