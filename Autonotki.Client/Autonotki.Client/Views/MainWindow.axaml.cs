using Avalonia.Controls;
using Avalonia.Interactivity;
using Autonotki.Client.Services;
using Autonotki.Client.ViewModels;

namespace Autonotki.Client.Views;

public partial class MainWindow : Window
{
    private readonly ApiService _api;

    public MainWindow()
    {
        InitializeComponent();
        _api = new ApiService();
        DataContext = new MainWindowViewModel(_api);
        Opened += MainWindow_Opened;
    }

    private async void MainWindow_Opened(object? sender, EventArgs e)
    {
        var loginWindow = new LoginWindow
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var result = await loginWindow.ShowDialog<bool?>(this);
        if (result == true && DataContext is MainWindowViewModel main)
        {
            main.CurrentPage = new HomeViewModel(_api, main);
        }
        else
        {
            Close();
        }
    }
}
