using ProjekatPozoriste.DTOs;

namespace ProjekatPozoriste.Services
{
    public interface IPredstavaService
    {
        Task<(bool Success, string Message)> DodajUcesnika(int predstavaId, int zaposleniId);

        Task<List<PredstavaDTO>> GetRepertoarAsync();

        Task<int> KreirajPredstavuAsync(NovaPredstavaDTO dto);

        Task<(bool Success, string Message)> DodajTerminAsync(NoviTerminDTO dto);

        Task<(bool Success, string Message)> DodajUcesnikaAsync(int predstavaId, int zaposleniId);

        Task<(bool Success, string Message)> ObrisiPredstavuAsync(int id);

        Task<List<object>> GetTerminiAsync(int predstavaId);

        Task<List<KartaDTO>> GetKarteZaTerminAsync(int terminId);

    }
}
