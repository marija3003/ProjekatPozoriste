using ProjekatPozoriste.DTOs;

namespace ProjekatPozoriste.Services
{
    public interface IPozoristeService
    {
        Task<List<PozoristeDTO>> GetPozoristaAsync();

        Task<string> KreirajPozoristeAsync(NovoPozoristeDTO dto);
    }
}
