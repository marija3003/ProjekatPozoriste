namespace ProjekatPozoriste.DTOs
{
    //kad se potvrdi kupovina, ovim sa fronta stize id prodane karte 
    public class KupiKartuDTO
    {
        public int KartaId { get; set; }
        public string ImeKupca { get; set; }
        //public int? ProdavacId { get; set; }
    }
}