namespace ProjekatPozoriste.Models
{
    public class Termin
    {

        public int Id { get; set; }
        public DateTime DatumVrijeme { get; set; }

        public int PredstavaId { get; set; }
        public Predstava Predstava { get; set; }
        public List<Karta> Karte { get; set; }
    }
}
