using System;
using System.Text.RegularExpressions;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Proxy;

public class ResponseFilter(ILog log)
{
    private readonly ILog log = log;
    private readonly Regex filterRegex = new Regex(@"1:\d+:.+#.+#.+", RegexOptions.Compiled);
    private readonly Regex errorRegex = new Regex(@"^-\d+$", RegexOptions.Compiled);
    private readonly Regex gdpsEmpty = new Regex(@"^###\d+:\d+:\d+#[a-f0-9]+$");

    public bool Filter(string response)
    {
        if (response.Contains("Error 1006"))
        {
            log.Error($"error 1006");
            return false;
        }

        if (response.Contains("<!DOCTYPE html>", StringComparison.Ordinal))
            return false;
        if (gdpsEmpty.IsMatch(response))
            return true;
        return filterRegex.IsMatch(response) || errorRegex.IsMatch(response);
    }
}
