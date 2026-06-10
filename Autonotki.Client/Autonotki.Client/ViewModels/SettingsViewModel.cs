using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Autonotki.Client.Services;

namespace Autonotki.Client.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService _settingsService;
    private readonly MainWindowViewModel? _main;
    public string[] AvailableThemes { get; } = new[] { "Light", "Dark" };
    // Playful dummy options (purely cosmetic)
    public string[] EasterEggOptions { get; } = new[] { "None", "Rainbow", "Chicken", "Disco" };

    [ObservableProperty] private bool _enableConfetti = false;
    [ObservableProperty] private string _favoriteUnicorn = "Sparkles";
    [ObservableProperty] private int _sillyNumber = 7;
    [ObservableProperty] private string _selectedEasterEgg = "None";

    [ObservableProperty] private string _selectedTheme = "Light";

    // Parameterless ctor kept for design-time
    public SettingsViewModel()
    {
        _settingsService = new SettingsService();
        SelectedTheme = _settingsService.Settings.Theme;
    }

    // Runtime ctor with navigation context
    public SettingsViewModel(MainWindowViewModel main) : this()
    {
        _main = main;
    }

    [RelayCommand]
    public void Save()
    {
        _settingsService.Settings = _settingsService.Settings with { Theme = SelectedTheme };
        _settingsService.Save();
        ThemeService.Instance.ApplyTheme(SelectedTheme);
        // After applying settings, return to the main list view
        _main?.NavigateToPrzegladaj();
    }

    [RelayCommand]
    public void Cancel()
    {
        // Navigate back to the main list view if possible
        _main?.NavigateToPrzegladaj();
    }
}
