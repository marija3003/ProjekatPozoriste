using ProjekatPozoriste.Enums;

namespace ProjekatPozoriste.Models
{
    public class Zaposleni
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public TipZaposlenog Tip { get; set; } 

        public int PozoristeId { get; set; }
        public Pozoriste Pozoriste { get; set; }
        public List<Predstava> Predstave { get; set; } = [];
    }

}