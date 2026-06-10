namespace Autonotki.Application.DTOs;

public record ZlecenieDto(
    int Id,
    string CarName,
    string? CarYear,
    string? Vin,
    string? TypNadwozia,
    string? Kolor,
    string? ServiceType,
    string? EstimatedCost,
    string? Status,
    string? DeadlineDisplay,
    string? ClientName,
    string? PhoneNumber
    , string? Miejscowosc
    , string? Adres
);

public record CreateZlecenieRequest(
    string Marka,
    string Model,
    string? RokProdukcji,
    string? Vin,
    string? TypNadwozia,
    string? Kolor,
    string? RodzajNaprawy,
    string? TerminRealizacji,
    string? SzacunkowyKoszt,
    string ImieNazwisko,
    string? Miejscowosc,
    string? Adres,
    string? Telefon
);

public record KalendarzDayDto(
    int DayNumber,
    bool HasPendingOrders,
    bool HasDoneOrders,
    bool IsToday,
    bool IsWeekend,
    bool IsOtherMonth
);

public record StatusUpdateRequest(string Status);
