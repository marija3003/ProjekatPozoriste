namespace ProjekatPozoriste.Models
{
    public class ProdajaKarte
    {
        public int Id { get; set; }

        public int KartaId { get; set; }
        public Karta Karta { get; set; }

        public string ImeKupca { get; set; }

        public int? ProdavacId { get; set; }
        public Zaposleni? Prodavac { get; set; }

        public DateTime DatumKupovine { get; set; }
    }
}
