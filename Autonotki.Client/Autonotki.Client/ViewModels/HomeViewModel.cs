using System.Collections.ObjectModel;
using Autonotki.Application.DTOs;
using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly MainWindowViewModel _main;

    [ObservableProperty] private string _monthName = "";
    [ObservableProperty] private string _todayLabel = "";

    public ObservableCollection<CalendarDayViewModel> Days { get; } = [];
    public ObservableCollection<ZlecenieMiniCardViewModel> TodaysOrders { get; } = [];
    public TopNavBarViewModel NavVM { get; }

    private DateTime _current = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public HomeViewModel(ApiService api, MainWindowViewModel main)
    {
        _api = api;
        _main = main;
        NavVM = new TopNavBarViewModel(main);
        var ci = new System.Globalization.CultureInfo("pl-PL");
        TodayLabel = DateTime.Today.ToString("d MMMM", ci);
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var ci = new System.Globalization.CultureInfo("pl-PL");
        MonthName = _current.ToString("MMMM", ci).ToUpper();

        var kalDays = await _api.GetKalendarzAsync(_current.Year, _current.Month);
        Days.Clear();
        int idx = 0;
        foreach (var d in kalDays)
        {
            DateOnly date;
            try { date = new DateOnly(_current.Year, _current.Month, d.DayNumber); }
            catch { date = DateOnly.FromDateTime(DateTime.Today); }

            Days.Add(new CalendarDayViewModel
            {
                DayNumber = d.DayNumber,
                HasPendingOrders = d.HasPendingOrders,
                HasDoneOrders = d.HasDoneOrders,
                IsToday = d.IsToday,
                IsWeekend = d.IsWeekend,
                IsOtherMonth = d.IsOtherMonth,
                Date = date,
                OnSelected = date => _ = LoadDayOrdersAsync(date)
            });
            idx++;
        }

        await LoadDayOrdersAsync(DateOnly.FromDateTime(DateTime.Today));
    }

    private async Task LoadDayOrdersAsync(DateOnly date)
    {
        var all = await _api.GetZleceniaAsync();
        TodaysOrders.Clear();
        foreach (var z in all.Where(z => z.DeadlineDisplay == date.ToString("dd.MM")))
            TodaysOrders.Add(new ZlecenieMiniCardViewModel(z));
    }
}
