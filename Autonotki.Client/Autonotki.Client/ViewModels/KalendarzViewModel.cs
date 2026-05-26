using System.Collections.ObjectModel;
using Autonotki.Application.DTOs;
using Autonotki.Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class KalendarzViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly MainWindowViewModel _main;
    private DateTime _current = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public TopNavBarViewModel NavVM { get; }
    [ObservableProperty] private string _monthName = "";
    public ObservableCollection<CalendarDayViewModel> Days { get; } = [];
    public ObservableCollection<ZlecenieCardViewModel> SelectedDayOrders { get; } = [];

    public KalendarzViewModel(ApiService api, MainWindowViewModel main)
    {
        _api = api; _main = main;
        NavVM = new TopNavBarViewModel(main);
        _ = LoadAsync();
    }

    [RelayCommand] public async Task PreviousMonth() { _current = _current.AddMonths(-1); await LoadAsync(); }
    [RelayCommand] public async Task NextMonth()     { _current = _current.AddMonths(1);  await LoadAsync(); }

    private async Task LoadAsync()
    {
        var ci = new System.Globalization.CultureInfo("pl-PL");
        MonthName = _current.ToString("MMMM", ci).ToUpper();

        var kalDays = await _api.GetKalendarzAsync(_current.Year, _current.Month);
        if (kalDays.Count == 0)
        {
            kalDays = GenerateLocalCalendarDays(_current.Year, _current.Month);
        }

        Days.Clear();
        foreach (var d in kalDays)
        {
            DateOnly date;
            if (!d.IsOtherMonth)
            {
                date = new DateOnly(_current.Year, _current.Month, Math.Clamp(d.DayNumber, 1, DateTime.DaysInMonth(_current.Year, _current.Month)));
            }
            else
            {
                // Fallback for previous/next month cells when the API does not include exact year/month info.
                var firstOfMonth = new DateTime(_current.Year, _current.Month, 1);
                var offset = ((int)firstOfMonth.DayOfWeek + 6) % 7;
                var cellIndex = Days.Count;
                var dayIndex = cellIndex - offset + 1;
                var dateTime = firstOfMonth.AddDays(dayIndex - 1);
                date = DateOnly.FromDateTime(dateTime);
            }

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
        }
    }

    private static List<KalendarzDayDto> GenerateLocalCalendarDays(int year, int month)
    {
        var days = new List<KalendarzDayDto>();
        var firstOfMonth = new DateTime(year, month, 1);
        var prevMonth = firstOfMonth.AddMonths(-1);
        var nextMonth = firstOfMonth.AddMonths(1);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var daysInPrevMonth = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
        var startOffset = ((int)firstOfMonth.DayOfWeek + 6) % 7;

        for (var cell = 0; cell < 42; cell++)
        {
            if (cell < startOffset)
            {
                var dayNumber = daysInPrevMonth - startOffset + cell + 1;
                var date = new DateTime(prevMonth.Year, prevMonth.Month, dayNumber);
                days.Add(new KalendarzDayDto(dayNumber, false, false, false, date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday, true));
            }
            else if (cell < startOffset + daysInMonth)
            {
                var dayNumber = cell - startOffset + 1;
                var date = new DateTime(year, month, dayNumber);
                days.Add(new KalendarzDayDto(dayNumber, false, false, date.Date == DateTime.Today.Date, date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday, false));
            }
            else
            {
                var dayNumber = cell - startOffset - daysInMonth + 1;
                var date = new DateTime(nextMonth.Year, nextMonth.Month, dayNumber);
                days.Add(new KalendarzDayDto(dayNumber, false, false, false, date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday, true));
            }
        }

        return days;
    }

    private async Task LoadDayOrdersAsync(DateOnly date)
    {
        var all = await _api.GetZleceniaAsync();
        SelectedDayOrders.Clear();
        foreach (var z in all.Where(z => z.DeadlineDisplay == date.ToString("dd.MM")))
            SelectedDayOrders.Add(new ZlecenieCardViewModel(z, _api, _main));
    }
}
