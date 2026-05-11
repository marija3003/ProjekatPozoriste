namespace ProjekatPozoriste.Models
{
    public class Predstava
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public int PozoristeId { get; set; }

        public string PutanjaSlike { get; set;}
        public Pozoriste Pozoriste { get; set; }
        public List<Zaposleni> Ucesnici { get; set; } = [];
        public List<Termin> Termini { get; set; } = [];
    }
}
