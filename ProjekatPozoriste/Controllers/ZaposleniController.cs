using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Enums;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaposleniController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        //pregled svih zaposlenih (zaposleni njihovi profili i povezanost s pozoristem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> GetZaposleni()
        {
            var zaposleni = await _context.Zaposleni
                .Include(z => z.Pozoriste)
                    .Select(z => new ZaposleniDTO
                    {
                        Id = z.Id,
                        ImePrezime = z.Ime + " " + z.Prezime,
                        Tip = z.Tip.ToString(),
                        PozoristeId = z.PozoristeId,
                        NazivPozorista = z.Pozoriste.Naziv
                    }).ToListAsync();

            return Ok(zaposleni);
        }

        [HttpPost]
        public async Task<IActionResult> KreirajZaposlenog(NoviZaposleniDTO dto)
        {
            if (!Enum.TryParse(typeof(TipZaposlenog), dto.Tip, out var tipEnum))
                return BadRequest("Nevalidan tip zaposlenog.");

            var novi = new Zaposleni
            {
                Ime = dto.Ime,
                Prezime = dto.Prezime,
                Tip = (TipZaposlenog)tipEnum,
                PozoristeId = dto.PozoristeId,
            };

            _context.Zaposleni.Add(novi);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Profil zaposlenog uspjesno kreiran." });
        }

        // filtriranje po tipu
        [HttpGet("Filter/{tip}")]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> GetPoTipu(string tip)
        {

            if (!Enum.TryParse(typeof(TipZaposlenog), tip, out var tipEnum))
            {
                return BadRequest("Nevalidan tip zaposlenog. Koristite G, R ili K.");
            }

            var rezultat = await _context.Zaposleni
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

            return Ok(rezultat);
        }

        //  pregled zaposlenih po pozoristu
        [HttpGet("Pozoriste/{pozoristeId}")]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> GetZaposleniPoPozoristu(int pozoristeId)
        {
            var zaposleni = await _context.Zaposleni
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

            return Ok(zaposleni);
        }

    }
}
