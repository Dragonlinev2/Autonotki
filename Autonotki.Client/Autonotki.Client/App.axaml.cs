using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Autonotki.Client.ViewModels;
using Autonotki.Client.Views;
using Autonotki.Client.Services;
using System.Linq;

namespace Autonotki.Client;

public partial class App : Avalonia.Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var toRemove = BindingPlugins.DataValidators
            .OfType<DataAnnotationsValidationPlugin>().ToArray();
        foreach (var p in toRemove)
            BindingPlugins.DataValidators.Remove(p);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            // Apply persisted theme
            try
            {
                var settings = new Services.SettingsService();
                ThemeService.Instance.ApplyTheme(settings.Settings.Theme);
            }
            catch { }
        }
        base.OnFrameworkInitializationCompleted();
    }
}
