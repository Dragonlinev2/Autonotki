using System.Collections.ObjectModel;
using Autonotki.Application.DTOs;
using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class ZleceniaViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly MainWindowViewModel _main;
    private List<ZlecenieDto> _all = [];

    public TopNavBarViewModel NavVM { get; }
    public ObservableCollection<ZlecenieCardViewModel> Orders { get; } = [];

    [ObservableProperty] private string _selectedDateDisplay =
        DateTime.Today.ToString("dd.MM.yyyy");
    [ObservableProperty] private bool _isEmpty;

    public ZleceniaViewModel(ApiService api, MainWindowViewModel main)
    {
        _api = api;
        _main = main;
        NavVM = new TopNavBarViewModel(main);
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        _all = await _api.GetZleceniaAsync();
        FilterAll();
    }

    [RelayCommand] public void FilterAll()        => Apply(null);
    [RelayCommand] public void FilterTodo()       => Apply("Do zrobienia");
    [RelayCommand] public void FilterInProgress() => Apply("W toku");
    [RelayCommand] public void FilterDone()       => Apply("Zrobione");

    private void Apply(string? filter)
    {
        Orders.Clear();
        var src = filter is null ? _all : _all.Where(z => z.Status == filter);
        foreach (var z in src)
            Orders.Add(new ZlecenieCardViewModel(z, _api, _main));
        IsEmpty = Orders.Count == 0;
    }
}
