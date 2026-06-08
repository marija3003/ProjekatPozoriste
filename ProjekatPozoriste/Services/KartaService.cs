using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Services
{
    public class KartaService(AppDbContext context) : IKartaService
    {

        private readonly AppDbContext _context = context;

        public async Task<List<Karta>> GetSjedistaAsync(int terminId)
        {
            return await _context.Karte
                .Where(k => k.TerminId == terminId)
                .ToListAsync();
        }

        // C#
        public async Task<(bool Success, string Message)> ProdajKartuAsync(KupiKartuDTO dto)
        {
            var karta = await _context.Karte.FindAsync(dto.KartaId);
            if (karta == null)
                return (false, "Karta nije pronađena.");
            if (karta.Stanje == Enums.StanjeKarte.Prodana)
                return (false, "Karta je vec prodana.");

            //if (dto.ProdavacId.HasValue)
            //{
            //    var prodavacExists = await _context.Zaposleni.AnyAsync(z => z.Id == dto.ProdavacId.Value);
            //    if (!prodavacExists)
            //        return (false, "Prodavac nije pronađen.");

            karta.Stanje = Enums.StanjeKarte.Prodana;

            var prodaja = new ProdajaKarte
            {
                KartaId = dto.KartaId,
                ImeKupca = dto.ImeKupca,
               // ProdavacId = dto.ProdavacId,
                DatumKupovine = DateTime.Now
            };

            _context.ProdajaKarata.Add(prodaja);
            await _context.SaveChangesAsync();
            return (true, "Karta prodana.");
        }

        public async Task<(bool Success, string Message)> StornirajKartuAsync(int kartaId)
        {
            var karta = await _context.Karte.FindAsync(kartaId);

            if (karta == null)
            return (false, "Karta nije pronađena.");

            karta.Stanje = Enums.StanjeKarte.Stornirana;

            await _context.SaveChangesAsync();

            return (true, "Prodaja karte uspješno stornirana.");
        }

        public async Task<List<object>> GetProdateKarteAsync()
        {
            return await _context.ProdajaKarata
                .Include(p => p.Karta)
                    .ThenInclude(k => k.Termin)
                        .ThenInclude(t => t.Predstava)
                .OrderByDescending(p => p.DatumKupovine)
                .Select(p => new {
                    KartaId = p.KartaId,
                    ImeKupca = p.ImeKupca,
                    DatumKupovine = p.DatumKupovine,
                    BrojSjedista = p.Karta.BrojSjedista,
                    Cijena = p.Karta.Cijena,
                    Predstava = p.Karta.Termin.Predstava.Naziv,
                    DatumPredstave = p.Karta.Termin.DatumVrijeme
                })
                .Cast<object>()
                .ToListAsync();
        }


    }
}
