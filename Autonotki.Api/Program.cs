using Autonotki.Application.DTOs;
using Autonotki.Infrastructure.Data;
using Autonotki.Infrastructure.Models;
using Autonotki.Infrastructure.Repositories;
using Autonotki.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("Postgres")
    ?? "Host=localhost;Port=5432;Database=AUTONOTKI;Username=postgres;Password=test123";

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));
builder.Services.AddScoped<ZlecenieRepository>();
builder.Services.AddSingleton<AuthService>(_ => new AuthService(conn));

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// ── Auth ─────────────────────────────────────────────────────────────────────
app.MapPost("/login", async (LoginRequest req, AuthService auth) =>
{
    if (string.IsNullOrWhiteSpace(req.Login) || string.IsNullOrWhiteSpace(req.Haslo))
        return Results.BadRequest(new { message = "Login i hasło są wymagane" });
    try
    {
        var rola = await auth.LoginAsync(req.Login, req.Haslo);
        return rola is null
            ? Results.Unauthorized()
            : Results.Ok(new { message = "Zalogowano poprawnie", rola });
    }
    catch (Exception ex)
    {
        return Results.Problem(title: "Błąd połączenia z bazą", detail: ex.Message);
    }
});

// ── Zlecenia ──────────────────────────────────────────────────────────────────
app.MapGet("/zlecenia", async (ZlecenieRepository repo) =>
    Results.Ok((await repo.GetAllAsync()).Select(ToDto)));

app.MapGet("/zlecenia/{id:int}", async (int id, ZlecenieRepository repo) =>
{
    var z = await repo.GetByIdAsync(id);
    return z is null ? Results.NotFound() : Results.Ok(ToDto(z));
});

app.MapPost("/zlecenia", async (CreateZlecenieRequest req, AppDbContext db) =>
{
    var parts = req.ImieNazwisko.Trim().Split(' ', 2);
    var imie = parts[0];
    var nazwisko = parts.Length > 1 ? parts[1] : "";

    var klient = await db.Klienci.FirstOrDefaultAsync(k =>
        k.Imie == imie && k.Nazwisko == nazwisko);

    if (klient is null)
    {
        klient = new Klient { Imie = imie, Nazwisko = nazwisko, NrTel = req.Telefon };
        db.Klienci.Add(klient);
        await db.SaveChangesAsync();
    }

    var pojazd = new Pojazd
    {
        IdKlient = klient.IdKlient,
        Marka = req.Marka,
        Model = req.Model,
        RokProdukcji = req.RokProdukcji,
        TypNadwozia = req.TypNadwozia,
        Kolor = req.Kolor,
        Vin = req.Vin
    };
    db.Pojazdy.Add(pojazd);
    await db.SaveChangesAsync();

    DateOnly? termin = null;
    if (!string.IsNullOrWhiteSpace(req.TerminRealizacji))
    {
        // obsługa formatów "dd.MM HH:mm" i "dd.MM.yyyy"
        var t = req.TerminRealizacji.Split(' ')[0].Trim();
        var parts2 = t.Split('.');
        if (parts2.Length >= 2 &&
            int.TryParse(parts2[0], out var d) &&
            int.TryParse(parts2[1], out var m))
        {
            var y = parts2.Length >= 3 && int.TryParse(parts2[2], out var yy)
                ? yy : DateTime.Today.Year;
            try { termin = new DateOnly(y, m, d); } catch { }
        }
    }

    decimal? koszt = null;
    if (!string.IsNullOrWhiteSpace(req.SzacunkowyKoszt) &&
        decimal.TryParse(req.SzacunkowyKoszt,
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var k))
        koszt = k;

    var zlecenie = new Zlecenie
    {
        IdPojazd = pojazd.IdPojazd,
        DataPrzyjecia = DateOnly.FromDateTime(DateTime.Today),
        DataZakonczenia = termin,
        Status = "Do zrobienia",
        Opis = req.RodzajNaprawy,
        KosztUslugi = koszt
    };
    db.Zlecenia.Add(zlecenie);
    await db.SaveChangesAsync();

    return Results.Created($"/zlecenia/{zlecenie.IdZlecenia}", new { id = zlecenie.IdZlecenia });
});

app.MapPut("/zlecenia/{id:int}/status", async (int id, StatusUpdateRequest req, ZlecenieRepository repo) =>
{
    var z = await repo.GetByIdAsync(id);
    if (z is null) return Results.NotFound();
    z.Status = req.Status;
    return await repo.UpdateAsync(z) ? Results.NoContent() : Results.Problem();
});

app.MapPut("/zlecenia/{id:int}", async (int id, CreateZlecenieRequest req, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(req.ImieNazwisko) || string.IsNullOrWhiteSpace(req.Marka) || string.IsNullOrWhiteSpace(req.Model))
        return Results.BadRequest(new { message = "Imię i nazwisko oraz marka i model są wymagane" });

    var z = await db.Zlecenia
        .Include(x => x.Pojazd).ThenInclude(p => p.Klient)
        .FirstOrDefaultAsync(x => x.IdZlecenia == id);
    if (z is null) return Results.NotFound();

    // update klient
    var parts = req.ImieNazwisko.Trim().Split(' ', 2);
    var imie = parts[0];
    var nazwisko = parts.Length > 1 ? parts[1] : "";
    var klient = z.Pojazd?.Klient;
    if (klient is not null)
    {
        klient.Imie = imie;
        klient.Nazwisko = nazwisko;
        klient.NrTel = req.Telefon;
        klient.Miejscowosc = req.Miejscowosc;
        klient.Adres = req.Adres;
    }

    // update pojazd
    if (z.Pojazd is not null)
    {
        z.Pojazd.Marka = req.Marka;
        z.Pojazd.Model = req.Model;
        z.Pojazd.Vin = req.Vin;
        z.Pojazd.RokProdukcji = req.RokProdukcji;
        z.Pojazd.TypNadwozia = req.TypNadwozia;
        z.Pojazd.Kolor = req.Kolor;
    }

    // update zlecenie
    z.Opis = req.RodzajNaprawy;
    decimal? koszt = null;
    if (!string.IsNullOrWhiteSpace(req.SzacunkowyKoszt) &&
        decimal.TryParse(req.SzacunkowyKoszt, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var k))
        koszt = k;
    z.KosztUslugi = koszt;

    DateOnly? termin = null;
    if (!string.IsNullOrWhiteSpace(req.TerminRealizacji))
    {
        var t = req.TerminRealizacji.Split(' ')[0].Trim();
        var parts2 = t.Split('.');
        if (parts2.Length >= 2 &&
            int.TryParse(parts2[0], out var d) &&
            int.TryParse(parts2[1], out var m))
        {
            var y = parts2.Length >= 3 && int.TryParse(parts2[2], out var yy)
                ? yy : DateTime.Today.Year;
            try { termin = new DateOnly(y, m, d); } catch { }
        }
    }
    z.DataZakonczenia = termin;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/zlecenia/{id:int}", async (int id, ZlecenieRepository repo) =>
    await repo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound());

// ── Kalendarz ─────────────────────────────────────────────────────────────────
app.MapGet("/kalendarz/{year:int}/{month:int}", async (int year, int month, ZlecenieRepository repo) =>
{
    var zlecenia = await repo.GetByMonthAsync(year, month);
    var today = DateOnly.FromDateTime(DateTime.Today);
    var firstDay = new DateOnly(year, month, 1);
    int offset = ((int)firstDay.DayOfWeek + 6) % 7; // pon=0
    int daysInMonth = DateTime.DaysInMonth(year, month);

    var days = new List<KalendarzDayDto>();

    // dni poprzedniego miesiąca
    if (offset > 0)
    {
        var prev = firstDay.AddMonths(-1);
        int prevTotal = DateTime.DaysInMonth(prev.Year, prev.Month);
        for (int i = offset - 1; i >= 0; i--)
        {
            int col = days.Count % 7;
            days.Add(new KalendarzDayDto(prevTotal - i, false, false, false, col is 5 or 6, true));
        }
    }

    // dni bieżącego miesiąca
    for (int d = 1; d <= daysInMonth; d++)
    {
        var date = new DateOnly(year, month, d);
        int col = days.Count % 7;
        bool hasPending = zlecenia.Any(z =>
            z.DataZakonczenia == date && z.Status is "Do zrobienia" or "W toku");
        bool hasDone = zlecenia.Any(z =>
            z.DataZakonczenia == date && z.Status == "Zrobione");
        days.Add(new KalendarzDayDto(d, hasPending, hasDone, date == today, col is 5 or 6, false));
    }

    // uzupełnij do 42
    int next = 1;
    while (days.Count < 42)
    {
        int col = days.Count % 7;
        days.Add(new KalendarzDayDto(next++, false, false, false, col is 5 or 6, true));
    }

    return Results.Ok(days);
});

app.Run();

// ── Helpers ───────────────────────────────────────────────────────────────────
static ZlecenieDto ToDto(Zlecenie z) => new(
    z.IdZlecenia,
    $"{z.Pojazd?.Marka} {z.Pojazd?.Model}".Trim(),
    z.Pojazd?.RokProdukcji,
    z.Pojazd?.Vin,
    z.Pojazd?.TypNadwozia,
    z.Pojazd?.Kolor,
    z.Opis,
    z.KosztUslugi?.ToString("F0"),
    z.Status ?? "Do zrobienia",
    z.DataZakonczenia?.ToString("dd.MM"),
    $"{z.Pojazd?.Klient?.Imie} {z.Pojazd?.Klient?.Nazwisko}".Trim(),
    z.Pojazd?.Klient?.NrTel
    , z.Pojazd?.Klient?.Miejscowosc
    , z.Pojazd?.Klient?.Adres
);

record LoginRequest(string Login, string Haslo);
