using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class TopNavBarViewModel(MainWindowViewModel main) : ViewModelBase
{
    [ObservableProperty] private int _notificationCount = 0;

    [RelayCommand] public void NavigateToDodajCommand()      => main.NavigateToDodaj();
    [RelayCommand] public void NavigateToEdytujCommand()     => main.NavigateToPrzegladaj();
    [RelayCommand] public void NavigateToPrzegladajCommand() => main.NavigateToPrzegladaj();
    [RelayCommand] public void NavigateToKalendarzCommand()  => main.NavigateToKalendarz();
    [RelayCommand] public void NavigateToUstawieniaCommand() { }
    [RelayCommand] public void OpenNotificationsCommand()    { }
    [RelayCommand] public void OpenProfileCommand()          { }
}
