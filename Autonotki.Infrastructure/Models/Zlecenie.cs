namespace Autonotki.Infrastructure.Models;

public class Zlecenie
{
    public int IdZlecenia { get; set; }
    public int IdPojazd { get; set; }
    public DateOnly? DataPrzyjecia { get; set; }
    public DateOnly? DataZakonczenia { get; set; }
    public string? Status { get; set; }
    public string? Opis { get; set; }
    public string? Uwagi { get; set; }
    public decimal? KosztUslugi { get; set; }
    public Pojazd Pojazd { get; set; } = null!;
}
