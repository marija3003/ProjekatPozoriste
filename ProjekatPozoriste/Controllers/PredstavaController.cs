using Microsoft.AspNetCore.Mvc;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Services;

namespace ProjekatPozoriste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredstavaController(IPredstavaService predstavaService) : ControllerBase
    {
        private readonly IPredstavaService _predstavaService = predstavaService;

        //dobavljanje repertoara, model u predstavadto 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredstavaDTO>>> GetRepertoar()
        {
            return Ok(await _predstavaService.GetRepertoarAsync());
        }

        [HttpPost]
        public async Task<IActionResult> KreirajPredstavu([FromForm] NovaPredstavaDTO dto)
        {
            var id = await _predstavaService.KreirajPredstavuAsync(dto);

            return Ok(new { id });
        }

        [HttpPost("termin")] 
        public async Task<IActionResult> DodajTermin(NoviTerminDTO dto)
        {
            var (Success, Message) = await _predstavaService.DodajTerminAsync(dto);

            if (!Success)
                return BadRequest(Message);

            return Ok(new { message = Message });
        }


        //dodavanje ucesnika, provjera u servisu
        [HttpPost("dodaj-ucesnika")]
        public async Task<IActionResult> DodajUcesnika(DodajUcesnikaDTO dto)
        {
            var (success, message) = await _predstavaService.DodajUcesnikaAsync(dto.PredstavaId, dto.ZaposleniId);

            if (!success) return BadRequest(message);
            return Ok(new { message });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ObrisiPredstavu(int id)
        {
            var (Success, Message) = await _predstavaService.ObrisiPredstavuAsync(id);

            if (!Success)
                return NotFound(Message);

            return Ok(new { message = Message });
        }

        [HttpGet("{predstavaId}/termini")]
        public async Task<IActionResult> GetTermini(int predstavaId)
        {
            return Ok(await _predstavaService.GetTerminiAsync(predstavaId));
        }


        //pregled karata za predstavu
        [HttpGet("{terminId}/karte")]
        public async Task<ActionResult<IEnumerable<KartaDTO>>> GetKarteZaTermin(int terminId)
        {
            return Ok(await _predstavaService.GetKarteZaTerminAsync(terminId));
        }

    }
}
