using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;

namespace ProjekatPozoriste.Services
{
    public class PredstavaService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<(bool Success, string Message)> DodajUcesnika(int predstavaId, int zaposleniId)
        {
            var predstava = await _context.Predstave
                .Include(p => p.Ucesnici)
                .FirstOrDefaultAsync(p => p.Id == predstavaId);


            var radnik = await _context.Zaposleni
                .FirstOrDefaultAsync(z => z.Id == zaposleniId);

            if (predstava == null || radnik == null)
                return (false, "Predstava ili zaposleni ne postoje.");

            // 1. Isto pozorište
            if (predstava.PozoristeId != radnik.PozoristeId)
                return (false, "Mora biti isto pozorište!");

            // 2. Mora postojati reditelj 
            if (predstava.Ucesnici.Count == 0 && radnik.Tip != Enums.TipZaposlenog.R)
                return (false, "Potrebno dodati reditelja!");

            // 3. Samo jedan reditelj
            if (radnik.Tip == Enums.TipZaposlenog.R &&
                predstava.Ucesnici.Any(u => u.Tip == Enums.TipZaposlenog.R))
                return (false, "Već postoji reditelj!");

            // 4. Max 2 kostimografa
            if (radnik.Tip == Enums.TipZaposlenog.K &&
                predstava.Ucesnici.Count(u => u.Tip == Enums.TipZaposlenog.K) >= 2)
                return (false, "Max 2 kostimografa!");

            // 5. duplikat zaštita
            if (predstava.Ucesnici.Any(u => u.Id == radnik.Id))
                return (false, "Već je dodat!");


            predstava.Ucesnici.Add(radnik);
            await _context.SaveChangesAsync();

            return (true, "Uspješno dodat učesnik.");
        }
    }
}
