using Autonotki.Application.DTOs;
using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class ZlecenieCardViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly MainWindowViewModel _main;

    public int Id { get; }
    public string CarName { get; }
    public string? CarYear { get; }
    public string? VIN { get; }
    public string? ServiceType { get; }
    public string? EstimatedCost { get; }
    public string? DeadlineDisplay { get; }
    public string? ClientName { get; }
    public string? PhoneNumber { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsInProgress))]
    [NotifyPropertyChangedFor(nameof(IsDone))]
    [NotifyPropertyChangedFor(nameof(IsTodo))]
    [NotifyPropertyChangedFor(nameof(StatusText))]
    private string _status;

    public bool IsInProgress => Status == "W toku";
    public bool IsDone       => Status == "Zrobione";
    public bool IsTodo       => Status == "Do zrobienia";
    public string StatusText => Status;

    public ZlecenieCardViewModel(ZlecenieDto dto, ApiService api, MainWindowViewModel main)
    {
        _api = api;
        _main = main;
        Id            = dto.Id;
        CarName       = dto.CarName;
        CarYear       = dto.CarYear;
        VIN           = dto.Vin;
        ServiceType   = dto.ServiceType;
        EstimatedCost = dto.EstimatedCost;
        DeadlineDisplay = dto.DeadlineDisplay;
        ClientName    = dto.ClientName;
        PhoneNumber   = dto.PhoneNumber;
        _status       = dto.Status ?? "Do zrobienia";
    }

    [RelayCommand]
    public async Task CycleStatus()
    {
        Status = Status switch
        {
            "Do zrobienia" => "W toku",
            "W toku"       => "Zrobione",
            _              => "Do zrobienia"
        };
        await _api.UpdateStatusAsync(Id, Status);
    }

    [RelayCommand]
    public void OpenDetails() => _main.NavigateToEdytuj(Id);
}
