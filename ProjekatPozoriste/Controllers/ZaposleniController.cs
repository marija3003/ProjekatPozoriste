using Microsoft.AspNetCore.Mvc;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Services;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaposleniController(IZaposleniService zaposleniService) : ControllerBase
    {
        private readonly IZaposleniService _zaposleniService = zaposleniService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> GetZaposleni()
        {
            return Ok(await _zaposleniService.GetZaposleniAsync());
        }

        [HttpPost]
        public async Task<IActionResult> KreirajZaposlenog(NoviZaposleniDTO dto)
        {
            var result = await _zaposleniService.KreirajZaposlenogAsync(dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = result.Message });
        }

        [HttpGet("Filter/{tip}")]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> GetPoTipu(string tip)
        {
            var rezultat = await _zaposleniService.GetPoTipuAsync(tip);

            if (rezultat.Count == 0)
                return BadRequest("Nevalidan tip zaposlenog.");

            return Ok(rezultat);
        }

        [HttpGet("Pozoriste/{pozoristeId}")]
        public async Task<ActionResult<IEnumerable<ZaposleniDTO>>> GetZaposleniPoPozoristu(int pozoristeId)
        {
            return Ok(await _zaposleniService.GetPoPozoristuAsync(pozoristeId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateZaposleni(int id, NoviZaposleniDTO dto)
        {
            var result = await _zaposleniService.UpdateZaposleniAsync(id, dto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ObrisiZaposlenog(int id)
        {
            var result = await _zaposleniService.ObrisiZaposlenogAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(new { message = result.Message });
        }
    }
}