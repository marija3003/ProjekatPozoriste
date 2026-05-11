using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Models;
using ProjekatPozoriste.Services;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredstavaController(AppDbContext context, PredstavaService predstavaService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly PredstavaService _predstavaService = predstavaService;

        //dobavljanje repertoara, model u predstavadto 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredstavaDTO>>> GetRepertoar()
        {
            var predstave = await _context.Predstave
                .Include(p => p.Pozoriste)
                .Include(p => p.Ucesnici)
                .ToListAsync();

            var repertoarDto = predstave.Select(p => new PredstavaDTO
            {
                Id = p.Id,
                Naziv = p.Naziv,
                NazivPozorista = p.Pozoriste.Naziv,
                PutanjaSlike = p.PutanjaSlike, 
                Reditelj = p.Ucesnici.FirstOrDefault(u => u.Tip == Enums.TipZaposlenog.R)?.Ime + " "
                         + p.Ucesnici.FirstOrDefault(u => u.Tip == Enums.TipZaposlenog.R)?.Prezime ?? "Nije dodijeljen",
                Glumci = p.Ucesnici.Where(u => u.Tip == Enums.TipZaposlenog.G).Select(u => u.Ime + " " + u.Prezime).ToList(),
                Kostimografi = p.Ucesnici.Where(u => u.Tip == Enums.TipZaposlenog.K).Select(u => u.Ime + " " + u.Prezime).ToList()
            }).ToList();

            return Ok(repertoarDto);
        }

        [HttpPost]
        public async Task<IActionResult> KreirajPredstavu([FromForm] NovaPredstavaDTO dto)
        {
            string? putanjaSlike = null;

            if (dto.Slika != null)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Slika.FileName);
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                { 
                    await dto.Slika.CopyToAsync(stream);
                }
                

                putanjaSlike = "images/" + fileName;
            }
            
            
            var novaPredstava = new Predstava
            {
                Naziv = dto.Naziv,
                PozoristeId = dto.PozoristeId,
                PutanjaSlike = putanjaSlike
            };

            _context.Predstave.Add(novaPredstava);
            await _context.SaveChangesAsync();

            return Ok(new { id = novaPredstava.Id });
        }

        [HttpPost("termin")] 
        public async Task<IActionResult> DodajTermin(NoviTerminDTO dto)
        {
            var termin = new Termin
            {
                PredstavaId = dto.PredstavaId,
                DatumVrijeme = dto.DatumVrijeme
            };

            _context.Termini.Add(termin);
            await _context.SaveChangesAsync();

          
            for (int i = 1; i <= 30; i++)
            {
                _context.Karte.Add(new Karta
                {
                    TerminId = termin.Id,
                    BrojSjedista = i,
                    IsProdata = false,
                    Cijena = 10
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Termin i karte su uspješno kreirani." });
        }


        //dodavanje ucesnika, provjera u servisu
        [HttpPost("dodaj-ucesnika")]
        public async Task<IActionResult> DodajUcesnika(DodajUcesnikaDTO dto)
        {
            var (success, message) = await _predstavaService.DodajUcesnika(dto.PredstavaId, dto.ZaposleniId);

            if (!success) return BadRequest(message);
            return Ok(new { message });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ObrisiPredstavu(int id)
        {
            var predstava = await _context.Predstave.FindAsync(id);

            if (predstava == null)
                return NotFound();

            
            var karte = _context.Karte
                .Where(k => k.Termin.PredstavaId == id);

            _context.Karte.RemoveRange(karte);

           
            var termini = _context.Termini
                .Where(t => t.PredstavaId == id);

            _context.Termini.RemoveRange(termini);

            _context.Predstave.Remove(predstava);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{predstavaId}/termini")]
        public async Task<IActionResult> GetTermini(int predstavaId)
        {
            var termini = await _context.Termini
                .Where(t => t.PredstavaId == predstavaId)
                .Select(t => new {
                    t.Id,
                    t.DatumVrijeme
                })
                .ToListAsync();
            return Ok(termini);
        }


        //pregled karata za predstavu
        [HttpGet("{terminId}/karte")]
        public async Task<ActionResult<IEnumerable<KartaDTO>>> GetKarteZaTermin(int terminId)
        {
            var karte = await _context.Karte
                .Where(k => k.TerminId == terminId)
                .Select(k => new KartaDTO
                {
                    Id = k.Id,
                    BrojSjedista = k.BrojSjedista,
                    Cijena = k.Cijena,
                    IsProdata = k.IsProdata
                })
                .ToListAsync();

            return Ok(karte);
        }

    }
}
