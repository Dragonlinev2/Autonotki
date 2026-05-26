namespace Autonotki.Infrastructure.Models;

public class Klient
{
    public int IdKlient { get; set; }
    public string Imie { get; set; } = "";
    public string Nazwisko { get; set; } = "";
    public string? NrTel { get; set; }
    public string? Email { get; set; }
    public ICollection<Pojazd> Pojazdy { get; set; } = [];
}
