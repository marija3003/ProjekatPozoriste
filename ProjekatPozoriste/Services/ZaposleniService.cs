using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Enums;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Services
{
    public class ZaposleniService(AppDbContext context) : IZaposleniService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<ZaposleniDTO>> GetZaposleniAsync()
        {
            return await _context.Zaposleni
                .Include(z => z.Pozoriste)
                .Select(z => new ZaposleniDTO
                {
                    Id = z.Id,
                    ImePrezime = z.Ime + " " + z.Prezime,
                    Tip = z.Tip.ToString(),
                    PozoristeId = z.PozoristeId,
                    NazivPozorista = z.Pozoriste.Naziv
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> KreirajZaposlenogAsync(NoviZaposleniDTO dto)
        {
            if (!Enum.TryParse(typeof(TipZaposlenog), dto.Tip, out var tipEnum))
                return (false, "Nevalidan tip zaposlenog.");

            var novi = new Zaposleni
            {
                Ime = dto.Ime,
                Prezime = dto.Prezime,
                Tip = (TipZaposlenog)tipEnum,
                PozoristeId = dto.PozoristeId
            };

            _context.Zaposleni.Add(novi);

            await _context.SaveChangesAsync();

            return (true, "Profil zaposlenog uspješno kreiran.");
        }

        public async Task<List<ZaposleniDTO>> GetPoTipuAsync(string tip)
        {
            if (!Enum.TryParse(typeof(TipZaposlenog), tip, out var tipEnum))
                return [];

            return await _context.Zaposleni
                .Include(z => z.Pozoriste)
                .Where(z => z.Tip == (TipZaposlenog)tipEnum)
                .Select(z => new ZaposleniDTO
                {
                    Id = z.Id,
                    ImePrezime = z.Ime + " " + z.Prezime,
                    Tip = z.Tip.ToString(),
                    PozoristeId = z.PozoristeId,
                    NazivPozorista = z.Pozoriste.Naziv
                })
                .ToListAsync();
        }

        public async Task<List<ZaposleniDTO>> GetPoPozoristuAsync(int pozoristeId)
        {
            return await _context.Zaposleni
                .Include(z => z.Pozoriste)
                .Where(z => z.PozoristeId == pozoristeId)
                .Select(z => new ZaposleniDTO
                {
                    Id = z.Id,
                    ImePrezime = z.Ime + " " + z.Prezime,
                    Tip = z.Tip.ToString(),
                    PozoristeId = z.PozoristeId,
                    NazivPozorista = z.Pozoriste.Naziv
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> UpdateZaposleniAsync(int id, NoviZaposleniDTO dto)
        {
            var zaposleni = await _context.Zaposleni.FindAsync(id);

            if (zaposleni == null)
                return (false, "Zaposleni nije pronađen.");

            if (!Enum.TryParse(typeof(TipZaposlenog), dto.Tip, out var tipEnum))
                return (false, "Nevalidan tip zaposlenog.");

            zaposleni.Ime = dto.Ime;
            zaposleni.Prezime = dto.Prezime;
            zaposleni.Tip = (TipZaposlenog)tipEnum;
            zaposleni.PozoristeId = dto.PozoristeId;

            await _context.SaveChangesAsync();

            return (true, "Podaci uspješno ažurirani.");
        }

        public async Task<(bool Success, string Message)> ObrisiZaposlenogAsync(int id)
        {
            var zaposleni = await _context.Zaposleni.FindAsync(id);

            if (zaposleni == null)
                return (false, "Zaposleni nije pronađen.");

            _context.Zaposleni.Remove(zaposleni);

            await _context.SaveChangesAsync();

            return (true, "Zaposleni obrisan.");
        }
    }
}