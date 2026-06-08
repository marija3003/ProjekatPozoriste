using Microsoft.AspNetCore.Mvc;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Services;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozoristeController(IPozoristeService pozoristeService) : ControllerBase
    {
        private readonly IPozoristeService _pozoristeService = pozoristeService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PozoristeDTO>>> GetPozorista()
        {
            var pozorista = await _pozoristeService.GetPozoristaAsync();
            return Ok(pozorista);
        }

        [HttpPost]
        public async Task<IActionResult> KreirajPozoriste(NovoPozoristeDTO dto)
        {
           var message = await _pozoristeService.KreirajPozoristeAsync(dto);

            return Ok(new { message });
        }
    }
}
