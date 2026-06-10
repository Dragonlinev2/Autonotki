using Avalonia.Controls;
using Avalonia.Interactivity;
using Autonotki.Client.Services;
using Autonotki.Client.ViewModels;

namespace Autonotki.Client.Views;

public partial class LoginWindow : Window
{
    private readonly ApiService _api;

    public LoginWindow()
    {
        InitializeComponent();
        _api = new ApiService();
        DataContext = new LoginWindowViewModel(_api);
    }

    private async void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is LoginWindowViewModel vm)
        {
            if (await vm.TryLoginAsync())
            {
                Close(true);
            }
            else if (!string.IsNullOrEmpty(vm.ErrorMessage))
            {
                var errorPopup = new ErrorPopup(vm.ErrorMessage)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                await errorPopup.ShowDialog(this);
            }
        }
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
