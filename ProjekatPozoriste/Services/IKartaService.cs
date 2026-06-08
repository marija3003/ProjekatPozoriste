using ProjekatPozoriste.DTOs;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Services
{
    public interface IKartaService
    {

        Task<List<Karta>> GetSjedistaAsync(int terminId);
        Task<(bool Success, string Message)> ProdajKartuAsync(KupiKartuDTO dto);

        Task<(bool Success, string Message)> StornirajKartuAsync(int kartaId);

        Task<List<object>> GetProdateKarteAsync();

    }
}
           