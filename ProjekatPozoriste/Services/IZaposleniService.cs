using ProjekatPozoriste.DTOs;

namespace ProjekatPozoriste.Services
{
    public interface IZaposleniService
    {
        Task<List<ZaposleniDTO>> GetZaposleniAsync();

        Task<(bool Success, string Message)> KreirajZaposlenogAsync(NoviZaposleniDTO dto);

        Task<List<ZaposleniDTO>> GetPoTipuAsync(string tip);

        Task<List<ZaposleniDTO>> GetPoPozoristuAsync(int pozoristeId);

        Task<(bool Success, string Message)> UpdateZaposleniAsync(int id, NoviZaposleniDTO dto);

        Task<(bool Success, string Message)> ObrisiZaposlenogAsync(int id);
       
    }
}
