using System;
using System.Text;
using GeometryDashAPI;
using Vostok.Logging.Abstractions;

namespace GeometryApp.Common;

public static class CryptExtensions
{
    public static (bool isSet, string? password) GetPasswordFromBase64(string? input)
    {
        if (input == null || input == "0")
            return (false, null);
        try
        {
            const string key = "26364";
            var bytes = Convert.FromBase64String(input);
            var result = Crypt.XOR(Encoding.ASCII.GetString(bytes), key);
            if (result.Length <= 1)
                return (false, null);
            return (result[0] == '1', result.Remove(0, 1));
        }
        catch (Exception e)
        {
            LogProvider.Get().ForContext(nameof(CryptExtensions)).Error(e, $"can't parse password: '{input}'");
            return (false, null);
        }
    }

    public static string? GetPasswordIfSetFromBase64(string? input)
    {
        var (isSet, password) = GetPasswordFromBase64(input);
        return isSet ? password : null;
    }
}
