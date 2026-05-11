using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozoristeController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PozoristeDTO>>> GetPozorista()
        {
            return Ok(await _context.Pozorista
                .Select(p => new PozoristeDTO
                    {
                        Id = p.Id,
                        Naziv = p.Naziv

                    }).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> KreirajPozoriste(NovoPozoristeDTO dto)
        {
            var pozoriste = new Pozoriste
            {
                Naziv = dto.Naziv
            };

            _context.Pozorista.Add(pozoriste);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pozoriste uspjesno kreirano" });
        }
    }
}
