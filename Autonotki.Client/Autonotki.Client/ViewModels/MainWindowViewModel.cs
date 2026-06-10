using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Autonotki.Client.Services;

namespace Autonotki.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ApiService _api;

    [ObservableProperty] private ViewModelBase _currentPage = null!;

    public MainWindowViewModel(ApiService api)
    {
        _api = api;
    }

    [RelayCommand] public void NavigateToHome()       => CurrentPage = new HomeViewModel(_api, this);
    [RelayCommand] public void NavigateToDodaj()      => CurrentPage = new DodajZlecenieViewModel(_api, this);
    [RelayCommand] public void NavigateToPrzegladaj() => CurrentPage = new ZleceniaViewModel(_api, this);
    [RelayCommand] public void NavigateToKalendarz()  => CurrentPage = new KalendarzViewModel(_api, this);
    [RelayCommand] public void NavigateToUstawienia() => CurrentPage = new SettingsViewModel(this);
    public void NavigateToEdytuj(int id)              => CurrentPage = new EdytujZlecenieViewModel(_api, this, id);
}
