using Autonotki.Application.DTOs;
using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class DodajZlecenieViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly MainWindowViewModel _main;
    public TopNavBarViewModel NavVM { get; }

    [ObservableProperty] private string _marka = "";
    [ObservableProperty] private string _model = "";
    [ObservableProperty] private string _rokProdukcji = "";
    [ObservableProperty] private string _vIN = "";
    [ObservableProperty] private string _typNadwozia = "";
    [ObservableProperty] private string _kolor = "";
    [ObservableProperty] private string _rodzajNaprawy = "";
    [ObservableProperty] private string _terminRealizacji = "";
    [ObservableProperty] private string _szacunkowyKoszt = "";
    [ObservableProperty] private string _miejscowosc = "";
    [ObservableProperty] private string _imieNazwisko = "";
    [ObservableProperty] private string _adres = "";
    [ObservableProperty] private string _telefon = "";

    public DodajZlecenieViewModel(ApiService api, MainWindowViewModel main)
    {
        _api = api; _main = main;
        NavVM = new TopNavBarViewModel(main);
    }

    [RelayCommand]
    public async Task Save()
    {
        if (string.IsNullOrWhiteSpace(ImieNazwisko)) return;
        var req = new CreateZlecenieRequest(
            Marka, Model, RokProdukcji, VIN, TypNadwozia, Kolor,
            RodzajNaprawy, TerminRealizacji, SzacunkowyKoszt,
            ImieNazwisko, Miejscowosc, Adres, Telefon);
        await _api.CreateZlecenieAsync(req);
        _main.NavigateToPrzegladaj();
    }
}
