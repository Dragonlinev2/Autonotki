using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Autonotki.Client.ViewModels;

public partial class CalendarDayViewModel : ViewModelBase
{
    public int DayNumber { get; init; }
    public bool HasPendingOrders { get; init; }
    public bool HasDoneOrders { get; init; }
    public bool IsToday { get; init; }
    public bool IsWeekend { get; init; }
    public bool IsOtherMonth { get; init; }
    public DateOnly Date { get; init; }

    public Action<DateOnly>? OnSelected { get; set; }

    [RelayCommand]
    public void SelectDay() => OnSelected?.Invoke(Date);
}
