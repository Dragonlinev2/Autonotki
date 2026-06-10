using Autonotki.Application.DTOs;
using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class EdytujZlecenieViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly MainWindowViewModel _main;
    private readonly int _id;
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
    [ObservableProperty] private string _imieNazwisko = "";
    [ObservableProperty] private string _miejscowosc = "";
    [ObservableProperty] private string _adres = "";
    [ObservableProperty] private string _telefon = "";

    public EdytujZlecenieViewModel(ApiService api, MainWindowViewModel main, int id)
    {
        _api = api; _main = main; _id = id;
        NavVM = new TopNavBarViewModel(main);
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var all = await _api.GetZleceniaAsync();
        var z = all.FirstOrDefault(x => x.Id == _id);
        if (z is null) return;
        var parts = z.CarName.Split(' ', 2);
        Marka            = parts[0];
        Model            = parts.Length > 1 ? parts[1] : "";
        VIN              = z.Vin ?? "";
        RokProdukcji     = z.CarYear ?? "";
        TypNadwozia      = z.TypNadwozia ?? "";
        Kolor            = z.Kolor ?? "";
        RodzajNaprawy    = z.ServiceType ?? "";
        SzacunkowyKoszt  = z.EstimatedCost ?? "";
        TerminRealizacji = z.DeadlineDisplay ?? "";
        ImieNazwisko     = z.ClientName ?? "";
        Telefon          = z.PhoneNumber ?? "";
        Miejscowosc      = z.Miejscowosc ?? "";
        Adres            = z.Adres ?? "";
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        if (string.IsNullOrWhiteSpace(ImieNazwisko) || string.IsNullOrWhiteSpace(Marka) || string.IsNullOrWhiteSpace(Model))
            return;

        var req = new CreateZlecenieRequest(
            Marka, Model, RokProdukcji, VIN, TypNadwozia, Kolor,
            RodzajNaprawy, TerminRealizacji, SzacunkowyKoszt,
            ImieNazwisko, Miejscowosc, Adres, Telefon);
        await _api.UpdateZlecenieAsync(_id, req);
        _main.NavigateToPrzegladaj();
    }
}
