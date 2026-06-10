using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Autonotki.Client.ViewModels;

public partial class LoginWindowViewModel : ViewModelBase
{
    private readonly ApiService _api;

    [ObservableProperty] private string _login = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string _errorMessage = "";

    public LoginWindowViewModel(ApiService api)
    {
        _api = api;
    }

    public async Task<bool> TryLoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Podaj login i hasło.";
            return false;
        }

        var role = await _api.LoginAsync(Login.Trim(), Password.Trim());

        if (role is null)
        {
            ErrorMessage = "Niepoprawny login lub hasło.";
            return false;
        }

        return true;
    }
}
