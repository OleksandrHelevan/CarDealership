using System;
using System.Collections.Generic;
using System.IO;

namespace CarDealership.util;

public static class DotEnv
{
    public static void Load(string? path = null, bool overwrite = false)
    {
        try
        {
            path ??= Path.Combine(AppContext.BaseDirectory, ".env");
            if (!File.Exists(path)) return;

            foreach (var line in File.ReadAllLines(path))
            {
                var trimmed = line?.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;
                if (trimmed.StartsWith("#")) continue;

                var idx = trimmed.IndexOf('=');
                if (idx <= 0) continue;

                var key = trimmed[..idx].Trim();
                var value = trimmed[(idx + 1)..].Trim();

                if (value.StartsWith("\"") && value.EndsWith("\""))
                    value = value[1..^1];

                if (!overwrite && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
                    continue;

                Environment.SetEnvironmentVariable(key, value);
            }
        }
        catch
        {
            // Ignore .env loading errors to avoid breaking app startup
        }
    }
}

