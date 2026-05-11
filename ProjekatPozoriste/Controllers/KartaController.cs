using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KartaController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;


        [HttpGet("termin/{id}")]
        public async Task<IActionResult> GetSjedista(int id)
        {
            var karte = await _context.Karte
                .Where(k => k.TerminId == id)
                .ToListAsync();
            return Ok(karte);
        }
        
        
        [HttpPost("prodaj")]
        public async Task<IActionResult> ProdajKartu(KupiKartuDTO dto)
        {
            var karta = await _context.Karte.FindAsync(dto.KartaId);

            if (karta == null)
                return NotFound();

            if (karta.IsProdata)
                return BadRequest("Karta je već prodana.");

            karta.IsProdata = true;

            var prodaja = new ProdajaKarte
            {
                KartaId = dto.KartaId,
                ImeKupca = dto.ImeKupca,
                ProdavacId = dto.ProdavacId,
                DatumKupovine = DateTime.Now
            };

            
            _context.Add(prodaja);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Karta prodana" });
        }

        [HttpPost("storniraj/{id}")]
        public async Task<IActionResult> StornirajKartu(int id)
        {
            var karta = await _context.Karte.FindAsync(id);
            if (karta == null)
                return NotFound();
            
            karta.IsProdata = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Prodaja karte uspjesno stornirana." });
        }

    }
}
