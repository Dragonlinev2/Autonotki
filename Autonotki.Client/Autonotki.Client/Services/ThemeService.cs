using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace Autonotki.Client.Services;

public class ThemeService
{
    public static ThemeService Instance { get; } = new ThemeService();

    private readonly Dictionary<string, Dictionary<string, string>> _themes = new()
    {
        ["Light"] = new Dictionary<string,string>
        {
            ["PageBackground"] = "#EEF0F3",
            ["CardBackground"] = "#FFFFFF",
            ["TextPrimary"] = "#1A1A2E",
            ["TextSecondary"] = "#5A6A7A",
        },
        ["Dark"] = new Dictionary<string,string>
        {
            ["PageBackground"] = "#1E1F23",
            ["CardBackground"] = "#252628",
            ["TextPrimary"] = "#EEEEEE",
            ["TextSecondary"] = "#BDBDBD",
        }
    };

    public void ApplyTheme(string name)
    {
        Console.WriteLine($"ThemeService: ApplyTheme called with '{name}'");
        var app = global::Avalonia.Application.Current;
        if (app == null || !app.Resources.Any()) return;
        if (!_themes.TryGetValue(name, out var map)) return;

        // Update resources inside merged dictionaries (where Colors.axaml defines them)
        foreach (var kv in map)
        {
            var color = Color.Parse(kv.Value);
            var brushKey = kv.Key + "Brush";

            var updated = false;
            // Try to update in merged dictionaries first using reflection (avoids compile-time type dependency)
            foreach (var mdObj in app.Resources.MergedDictionaries)
            {
                var mdType = mdObj.GetType();
                var containsMethod = mdType.GetMethod("ContainsKey", new[] { typeof(object) });
                var indexer = mdType.GetProperty("Item");
                if (containsMethod != null && indexer != null)
                {
                    var hasColor = (bool)containsMethod.Invoke(mdObj, new object[] { kv.Key });
                    if (hasColor)
                    {
                        indexer.SetValue(mdObj, color, new object[] { kv.Key });
                        updated = true;
                        Console.WriteLine($"ThemeService: updated color '{kv.Key}' in merged dictionary");
                    }

                    var hasBrush = (bool)containsMethod.Invoke(mdObj, new object[] { brushKey });
                    if (hasBrush)
                    {
                        indexer.SetValue(mdObj, new SolidColorBrush(color), new object[] { brushKey });
                        updated = true;
                        Console.WriteLine($"ThemeService: updated brush '{brushKey}' in merged dictionary");
                    }
                }
            }

            // Fallback to top-level resources if not found in merged dictionaries
            if (!updated)
            {
                app.Resources[kv.Key] = color;
                app.Resources[brushKey] = new SolidColorBrush(color);
                Console.WriteLine($"ThemeService: updated top-level resources for '{kv.Key}' and '{brushKey}'");
            }
        }
    }
}
