# Autonotki.Client — GUI (AXAML)

Kompletny interfejs graficzny dla projektu **Autonotkiv2** w Avalonia UI.
Odwzorowuje wszystkie 5 widoków z dostarczonych screenshotów.

---

## Struktura plików

```
Autonotki.Client/
├── App.axaml                          ← Entry point, ładuje style
│
├── MainWindow.axaml                   ← Shell okna, ContentControl do nawigacji
│
├── Styles/
│   ├── Colors.axaml                   ← Paleta kolorów (Resource Dictionary)
│   ├── Typography.axaml               ← Style TextBlock (PageHeader, Body, itp.)
│   ├── Controls.axaml                 ← Style przycisków, TextBox, kart
│   └── GlobalStyles.axaml             ← Globalne selektory, klasy pomocnicze
│
├── Controls/  (wielokrotnego użytku)
│   ├── TopNavBar.axaml                ← Pasek nawigacji (granatowy, wspólny)
│   ├── MiniCalendar.axaml             ← Miniaturowy kalendarz (strona główna)
│   ├── ZlecenieMiniCard.axaml         ← Karta zlecenia w sidebarze (strona główna)
│   ├── ZlecenieCard.axaml             ← Pełna karta zlecenia (lista zleceń)
│   └── ZlecenieFormPanel.axaml        ← Formularz auta + klienta (Dodaj / Edytuj)
│
└── Views/
    ├── HomeView.axaml                 ← Strona Główna
    ├── KalendarzView.axaml            ← Kalendarz (pełny) + zlecenia dnia
    ├── ZleceniaView.axaml             ← Przeglądaj zlecenia (z filtrami)
    ├── DodajZlecenieView.axaml        ← Dodaj zlecenie (pusty formularz)
    └── EdytujZlecenieView.axaml       ← Edytuj zlecenie (wypełniony formularz)
```

---

## Paleta kolorów

| Token                  | Wartość     | Użycie                          |
|------------------------|-------------|----------------------------------|
| `NavyBlue`             | `#2C3E6B`   | Pasek nav, przycisk Primary      |
| `OrangeStatus`         | `#F5A623`   | Status "W toku", kropki kalend.  |
| `GreenStatus`          | `#27AE60`   | Status "Zrobione", kropki kalend.|
| `PageBackground`       | `#EEF0F3`   | Tło stron                       |
| `CardBackground`       | `#FFFFFF`   | Tło kart i formularzy            |
| `CardBorder`           | `#D8DCE6`   | Obramowania                     |

---

## Style przycisków

| Klucz                      | Wygląd                                     |
|----------------------------|--------------------------------------------|
| `NavButtonStyle`           | Tekst biały, tło transparentne/hover ciemny|
| `OrangeButtonStyle`        | Pomarańczowy, zaokrąglony (status W toku)  |
| `GreenButtonStyle`         | Zielony, zaokrąglony (status Zrobione)     |
| `OutlineButtonStyle`       | Obrys szary, transparentne tło             |
| `PrimaryButtonStyle`       | Granatowy, prostokąt (Zapisz)              |
| `FilterButtonStyle`        | Obrys szary (nieaktywny filtr)             |
| `FilterButtonSelectedStyle`| Granatowy (aktywny filtr)                  |
| `CalendarArrowStyle`       | < > nawigacja kalendarza                   |

---

## ViewModels (oczekiwane interfejsy)

### TopNavBarViewModel
```csharp
ICommand NavigateToDodajCommand
ICommand NavigateToEdytujCommand
ICommand NavigateToPrzegladajCommand
ICommand NavigateToKalendarzCommand
ICommand NavigateToUstawieniaCommand
ICommand OpenNotificationsCommand
ICommand OpenProfileCommand
int NotificationCount
```

### MiniCalendarViewModel / KalendarzViewModel
```csharp
string MonthName           // "MAJ"
ObservableCollection<CalendarDayViewModel> Days
ICommand PreviousMonthCommand
ICommand NextMonthCommand
// KalendarzViewModel also:
ObservableCollection<ZlecenieKalendarzVM> SelectedDayOrders
```

### CalendarDayViewModel
```csharp
string DayNumber
bool HasPendingOrders      // orange dot
bool HasDoneOrders         // green dot
bool IsToday
bool IsWeekend
ICommand SelectDayCommand
```

### ZlecenieCardViewModel
```csharp
string CarName, CarYear, VIN, ServiceType, EstimatedCost
string DeadlineDisplay, ClientName, PhoneNumber
bool IsInProgress, IsDone, IsTodo
string StatusText
ICommand CycleStatusCommand
ICommand OpenDetailsCommand
```

### ZlecenieFormViewModel (Dodaj / Edytuj)
```csharp
string Miejscowosc, ImieNazwisko, Adres, Telefon
string Marka, Model, RokProdukcji, VIN, TypNadwozia
string Kolor, RodzajNaprawy, TerminRealizacji, SzacunkowyKoszt
IBitmap? PhotoSource
ICommand AddPhotoCommand
```

---

## Nawigacja (MainWindow)

`MainWindowViewModel.CurrentPage` to obiekt aktywnego widoku.
Nawigacja przez `NavigationService.NavigateTo<TView>()` lub
przez prostą właściwość `CurrentPage = new HomeView { DataContext = vm }`.

---

## Integracja z istniejącym projektem

1. Skopiuj folder `Autonotki.Client/` do solucji.
2. Dodaj referencje NuGet: `Avalonia`, `Avalonia.Desktop`, `Avalonia.Themes.Fluent`.
3. Podłącz `App.axaml` jako `StartupUri` lub przez `AppBuilder`.
4. Zaimplementuj ViewModels wg interfejsów powyżej (ReactiveUI lub CommunityToolkit.Mvvm).
5. Podłącz `NavigationService` do przycisków w `TopNavBar`.
