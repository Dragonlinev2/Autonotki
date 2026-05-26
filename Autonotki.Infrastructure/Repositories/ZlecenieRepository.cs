using Autonotki.Infrastructure.Data;
using Autonotki.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Autonotki.Infrastructure.Repositories;

public class ZlecenieRepository(AppDbContext db)
{
    public Task<List<Zlecenie>> GetAllAsync() =>
        db.Zlecenia
          .Include(z => z.Pojazd).ThenInclude(p => p.Klient)
          .OrderBy(z => z.DataZakonczenia)
          .ToListAsync();

    public Task<List<Zlecenie>> GetByMonthAsync(int year, int month) =>
        db.Zlecenia
          .Include(z => z.Pojazd).ThenInclude(p => p.Klient)
          .Where(z => z.DataZakonczenia.HasValue
                   && z.DataZakonczenia.Value.Year == year
                   && z.DataZakonczenia.Value.Month == month)
          .ToListAsync();

    public Task<Zlecenie?> GetByIdAsync(int id) =>
        db.Zlecenia
          .Include(z => z.Pojazd).ThenInclude(p => p.Klient)
          .FirstOrDefaultAsync(z => z.IdZlecenia == id);

    public async Task<Zlecenie> CreateAsync(Zlecenie z)
    {
        db.Zlecenia.Add(z);
        await db.SaveChangesAsync();
        return z;
    }

    public async Task<bool> UpdateAsync(Zlecenie z)
    {
        db.Zlecenia.Update(z);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var z = await db.Zlecenia.FindAsync(id);
        if (z is null) return false;
        db.Zlecenia.Remove(z);
        return await db.SaveChangesAsync() > 0;
    }
}
