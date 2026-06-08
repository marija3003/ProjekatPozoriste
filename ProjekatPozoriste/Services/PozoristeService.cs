using Microsoft.EntityFrameworkCore;
using ProjekatPozoriste.Data;
using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Services
{
    public class PozoristeService(AppDbContext context) : IPozoristeService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<PozoristeDTO>> GetPozoristaAsync()
        {
            return await _context.Pozorista
                .Select(p => new PozoristeDTO
                {
                    Id = p.Id,
                    Naziv = p.Naziv
                }).ToListAsync();
        }

        public async Task<string> KreirajPozoristeAsync(NovoPozoristeDTO dto)
        {
            var pozoriste = new Pozoriste
            {
                Naziv = dto.Naziv

            };

            _context.Pozorista.Add(pozoriste);
            await _context.SaveChangesAsync();

            return "Pozorište uspješno kreirano.";
        }
    }
}
