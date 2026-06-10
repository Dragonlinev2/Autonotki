namespace Autonotki.Infrastructure.Models;

public class Pojazd
{
    public int IdPojazd { get; set; }
    public int IdKlient { get; set; }
    public string? Marka { get; set; }
    public string? Model { get; set; }
    public string? RokProdukcji { get; set; }
    public string? TypNadwozia { get; set; }
    public string? Kolor { get; set; }
    public string? NrRejestracyjny { get; set; }
    public string? Vin { get; set; }
    public Klient Klient { get; set; } = null!;
    public ICollection<Zlecenie> Zlecenia { get; set; } = [];
}
