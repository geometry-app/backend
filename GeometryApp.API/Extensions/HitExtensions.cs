using System;
using System.Linq;
using GeometryApp.Common;
using Nest;

namespace GeometryApp.API.Extensions;

public static class HitExtensions
{
    public static HighlightString GetHighlighted<T>(this IHit<T> hit, string name, Func<T, string> select) where T : class
    {
        return (hit.Highlight.TryGetValue(name, out var text)
            ? text.FirstOrDefault() ?? select(hit.Source)
            : select(hit.Source)).ToHighlightString();
    }
}
