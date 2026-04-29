using ProjekatPozoriste.Enums;
using System.Text.Json.Serialization;

namespace ProjekatPozoriste.Models
{
    public class Zaposleni
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public TipZaposlenog Tip { get; set; } // Glumac, Reditelj, Kostimograf

        public int PozoristeId { get; set; }
        [JsonIgnore] // Da se izbjegnu ciklične reference u API-ju
        public Pozoriste Pozoriste { get; set; }
    }
}
