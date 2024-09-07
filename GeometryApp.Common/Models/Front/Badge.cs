using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GeometryApp.Common.Models.Front;

[DataContract]
public class Badge
{
    [DataMember]
    public string Text { get; set; } = null!;
    [DataMember]
    public string? Link { get; set; }
}

public class BadgeFormatter
{
    public static Badge Format(string id, Dictionary<string, object>? data)
    {
        if (id == "pointcreate_hardest")
        {
            if (data != null && data.TryGetValue("position", out var position))
            {
                return new Badge()
                {
                    Text = $"top {position} hardest by pointcreate",
                    Link = $"https://pointercrate.com/demonlist/{position}"
                };
            }
        }

        if (id == "impossible_list")
        {
            if (data != null && data.TryGetValue("position", out var position))
            {
                return new Badge()
                {
                    Text = $"top {position} by Impossible List",
                    Link = "https://www.impossible-list.com/home"
                };
            }
        }

        if (id == "shitty")
        {
            if (data != null && data.TryGetValue("position", out var position))
            {
                return new Badge()
                {
                    Text = "shitty list",
                    Link = "https://tsl.pages.dev/#/"
                };
            }
        }

        throw new InvalidOperationException($"unknown badge: {id}");
    }
}
