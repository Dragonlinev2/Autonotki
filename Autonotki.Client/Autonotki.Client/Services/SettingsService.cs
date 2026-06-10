using System.IO;
using System.Text.Json;

namespace Autonotki.Client.Services;

public record AppSettings
{
    public string Theme { get; init; } = "Light";
}

public class SettingsService
{
    private readonly string _path;
    public AppSettings Settings { get; set; }

    public SettingsService()
    {
        var cfgDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutonotkiClient");
        Directory.CreateDirectory(cfgDir);
        _path = Path.Combine(cfgDir, "settings.json");
        Settings = Load();
    }

    private AppSettings Load()
    {
        if (!File.Exists(_path)) return new AppSettings();
        try
        {
            var txt = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<AppSettings>(txt) ?? new AppSettings();
        }
        catch { return new AppSettings(); }
    }

    public void Save()
    {
        var txt = JsonSerializer.Serialize(Settings, new JsonSerializerOptions{WriteIndented=true});
        File.WriteAllText(_path, txt);
    }
}
