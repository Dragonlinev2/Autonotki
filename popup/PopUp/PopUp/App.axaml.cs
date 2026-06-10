using System;
using System.Xml;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using PopUp;

namespace PopUp;

public class App : Application
{
    private MainWindow _mainWindow = new ();
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow.SetMessage("Uwaga, wprowadziłeś niepoprawną wartość!");
            _mainWindow.SetIcon("⚠");
            _mainWindow.SetStyle(Brushes.Yellow);
            _mainWindow.AddButton("OK");
            _mainWindow.SetComponent();
            desktop.MainWindow = _mainWindow;
            _mainWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }
}