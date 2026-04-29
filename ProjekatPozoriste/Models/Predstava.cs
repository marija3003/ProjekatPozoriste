namespace ProjekatPozoriste.Models
{
    public class Predstava
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public int PozoristeId { get; set; }
        public Pozoriste Pozoriste { get; set; }

        // Veza sa zaposlenima koji rade na ovoj predstavi
        public List<Zaposleni> Ucesnici { get; set; } = [];
    }
}
