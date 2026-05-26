using Autonotki.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Autonotki.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Klient> Klienci => Set<Klient>();
    public DbSet<Pojazd> Pojazdy => Set<Pojazd>();
    public DbSet<Zlecenie> Zlecenia => Set<Zlecenie>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Klient>(e =>
        {
            e.ToTable("KLIENCI");
            e.HasKey(x => x.IdKlient);
            e.Property(x => x.IdKlient).HasColumnName("id_klient");
            e.Property(x => x.Imie).HasColumnName("imie");
            e.Property(x => x.Nazwisko).HasColumnName("nazwisko");
            e.Property(x => x.NrTel).HasColumnName("nr_tel");
            e.Property(x => x.Email).HasColumnName("email");
        });

        b.Entity<Pojazd>(e =>
        {
            e.ToTable("POJAZDY");
            e.HasKey(x => x.IdPojazd);
            e.Property(x => x.IdPojazd).HasColumnName("id_pojazd");
            e.Property(x => x.IdKlient).HasColumnName("id_klient");
            e.Property(x => x.Marka).HasColumnName("marka");
            e.Property(x => x.Model).HasColumnName("model");
            e.Property(x => x.NrRejestracyjny).HasColumnName("nr_rejestracyjny");
            e.Property(x => x.Vin).HasColumnName("vin");
            e.HasOne(x => x.Klient)
             .WithMany(k => k.Pojazdy)
             .HasForeignKey(x => x.IdKlient);
        });

        b.Entity<Zlecenie>(e =>
        {
            e.ToTable("ZLECENIA");
            e.HasKey(x => x.IdZlecenia);
            e.Property(x => x.IdZlecenia).HasColumnName("id_zlecenia");
            e.Property(x => x.IdPojazd).HasColumnName("id_pojazd");
            e.Property(x => x.DataPrzyjecia).HasColumnName("data_przyjecia");
            e.Property(x => x.DataZakonczenia).HasColumnName("data_zakonczenia");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.Opis).HasColumnName("opis");
            e.Property(x => x.Uwagi).HasColumnName("uwagi");
            e.Property(x => x.KosztUslugi).HasColumnName("koszt_uslugi");
            e.HasOne(x => x.Pojazd)
             .WithMany(p => p.Zlecenia)
             .HasForeignKey(x => x.IdPojazd);
        });
    }
}
