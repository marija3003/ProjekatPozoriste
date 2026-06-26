using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Enums;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Services
{
    public class PredstavaService(AppDbContext context, IWebHostEnvironment env) : IPredstavaService
    {
        public async Task<List<PredstavaDTO>> GetRepertoarAsync()
        {
            var predstave = await context.Predstave
                .Include(p => p.Pozoriste)
                .Include(p => p.Ucesnici)
                .ToListAsync();

            return [.. predstave.Select(p => new PredstavaDTO
            {
                Id = p.Id,
                Naziv = p.Naziv,
                NazivPozorista = p.Pozoriste.Naziv,
                PutanjaSlike = p.PutanjaSlike,

                Reditelj =
                    p.Ucesnici.FirstOrDefault(u => u.Tip == TipZaposlenog.R)?.Ime + " " +
                    p.Ucesnici.FirstOrDefault(u => u.Tip == TipZaposlenog.R)?.Prezime
                    ?? "Nije dodijeljen",

                Glumci = [.. p.Ucesnici
                    .Where(u => u.Tip == TipZaposlenog.G)
                    .Select(u => u.Ime + " " + u.Prezime)],

                Kostimografi = [.. p.Ucesnici
                    .Where(u => u.Tip == TipZaposlenog.K)
                    .Select(u => u.Ime + " " + u.Prezime)]
            })];
        }

        
        public async Task<int> KreirajPredstavuAsync(NovaPredstavaDTO dto)
        {
            string? putanjaSlike = null;

            if (dto.Slika != null)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Predstave");
                Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Slika.FileName);
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.Slika.CopyToAsync(stream);
                }

                putanjaSlike = $"Uploads/Predstave/{fileName}";
            }

            var predstava = new Predstava
            {
                Naziv = dto.Naziv,
                PozoristeId = dto.PozoristeId,
                PutanjaSlike = putanjaSlike
            };

            context.Predstave.Add(predstava);
            await context.SaveChangesAsync();

            foreach (var datum in dto.Termini)
            {
                await KreirajTerminIKarte(predstava.Id, datum);
            }

            return predstava.Id;
        }

        
        public async Task<(bool Success, string Message)> DodajTerminAsync(NoviTerminDTO dto)
        {
            await KreirajTerminIKarte(dto.PredstavaId, dto.DatumVrijeme);
            return (true, "Termin i karte uspješno kreirani.");
        }

        private async Task KreirajTerminIKarte(int predstavaId, DateTime datum)
        {
            var termin = new Termin
            {
                PredstavaId = predstavaId,
                DatumVrijeme = datum
            };

            context.Termini.Add(termin);
            await context.SaveChangesAsync();

            var karte = new List<Karta>();

            for (int i = 1; i <= 30; i++)
            {
                karte.Add(new Karta
                {
                    TerminId = termin.Id,
                    BrojSjedista = i,
                    Stanje = StanjeKarte.Slobodna,
                    Cijena = 10
                });
            }

            context.Karte.AddRange(karte);
            await context.SaveChangesAsync();
        }

        
        public async Task<(bool Success, string Message)> ObrisiPredstavuAsync(int id)
        {
            var predstava = await context.Predstave
                .Include(p => p.Ucesnici)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (predstava == null)
                return (false, "Predstava nije pronađena.");

            var termini = await context.Termini
                .Where(t => t.PredstavaId == id)
                .ToListAsync();

            var terminIds = termini.Select(t => t.Id).ToList();

            var karte = await context.Karte
                .Where(k => terminIds.Contains(k.TerminId))
                .ToListAsync();

            context.Karte.RemoveRange(karte);
            context.Termini.RemoveRange(termini);

            predstava.Ucesnici.Clear();

            context.Predstave.Remove(predstava);

            await context.SaveChangesAsync();

            return (true, "Predstava uspješno obrisana.");
        }

        
        public async Task<List<object>> GetTerminiAsync(int predstavaId)
        {
            return await context.Termini
                .Where(t => t.PredstavaId == predstavaId)
                .Select(t => new
                {
                    t.Id,
                    t.DatumVrijeme
                })
                .ToListAsync<object>();
        }

     
        public async Task<List<KartaDTO>> GetKarteZaTerminAsync(int terminId)
        {
            return await context.Karte
                .Where(k => k.TerminId == terminId)
                .Select(k => new KartaDTO
                {
                    Id = k.Id,
                    BrojSjedista = k.BrojSjedista,
                    Cijena = k.Cijena,
                    Stanje = k.Stanje
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> DodajUcesnikaAsync(int predstavaId, int zaposleniId)
        {
            var predstava = await context.Predstave
                .Include(p => p.Ucesnici)
                .FirstOrDefaultAsync(p => p.Id == predstavaId);


            var radnik = await context.Zaposleni
                .FirstOrDefaultAsync(z => z.Id == zaposleniId);

            if (predstava == null || radnik == null)
                return (false, "Predstava ili zaposleni ne postoje.");

            // 1. Isto pozorište
            if (predstava.PozoristeId != radnik.PozoristeId)
                return (false, "Mora biti isto pozorište!");

            // 2. Mora postojati reditelj 
            if (predstava.Ucesnici.Count == 0 && radnik.Tip != TipZaposlenog.R)
                return (false, "Potrebno dodati reditelja!");

            // 3. Samo jedan reditelj
            if (radnik.Tip == TipZaposlenog.R &&
                predstava.Ucesnici.Any(u => u.Tip == TipZaposlenog.R))
                return (false, "Već postoji reditelj!");

            // 4. Max 2 kostimografa
            if (radnik.Tip == TipZaposlenog.K &&
                predstava.Ucesnici.Count(u => u.Tip == TipZaposlenog.K) >= 2)
                return (false, "Max 2 kostimografa!");

            // 5. duplikat zaštita 
            if (predstava.Ucesnici.Any(u => u.Id == radnik.Id))
                return (false, "Već je dodan!");


            predstava.Ucesnici.Add(radnik);
            await context.SaveChangesAsync();

            return (true, "Uspješno dodan učesnik.");
        }

        //public async Task<(bool Success, string Message)> DodajUcesnikaAsync(int predstavaId, int zaposleniId)
        //{
        //    var predstava = await context.Predstave
        //        .Include(p => p.Ucesnici)
        //        .FirstOrDefaultAsync(p => p.Id == predstavaId);

        //    var zaposleni = await context.Zaposleni.FindAsync(zaposleniId);

        //    if (predstava == null || zaposleni == null)
        //        return (false, "Predstava ili zaposleni ne postoje.");

        //    if (zaposleni.PozoristeId != predstava.PozoristeId)
        //        return (false, "Mora biti isto pozorište!");

        //    if (predstava.Ucesnici.Any(u => u.Id == zaposleniId))
        //        return (false, "Zaposleni je već dodan.");

        //    predstava.Ucesnici.Add(zaposleni);

        //    await context.SaveChangesAsync();

        //    return (true, "Učesnik uspješno dodan.");
        //}
    }
}