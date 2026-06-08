using Microsoft.AspNetCore.Mvc;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Services;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KartaController(IKartaService kartaService) : ControllerBase
    {
        private readonly IKartaService _kartaService = kartaService;

        [HttpGet("termin/{id}")]
        public async Task<IActionResult> GetSjedista(int id)
        {
            var karte = await _kartaService.GetSjedistaAsync(id);
            return Ok(karte);
        }
        
        
        [HttpPost("prodaj")]
        public async Task<IActionResult> ProdajKartu(KupiKartuDTO dto)
        {
            var (Success, Message) = await _kartaService.ProdajKartuAsync(dto);

            if (!Success)
                return BadRequest(Message);

            return Ok(new { message = Message });
        }

        [HttpPost("storniraj/{id}")]
        public async Task<IActionResult> StornirajKartu(int id)
        {
            var (Success, Message) = await _kartaService.StornirajKartuAsync(id);

            if (!Success)
                return BadRequest(Message);

            return Ok(new { message = Message });
        }

        [HttpGet("prodane")]
        public async Task<IActionResult> GetProdane()
        {
            var prodane = await _kartaService.GetProdateKarteAsync();
            return Ok(prodane);
        }

    }
}
