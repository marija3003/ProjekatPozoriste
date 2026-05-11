namespace ProjekatPozoriste.DTOs
{
    //prikaz svih zaposlenih u pozoristu 
    public class ZaposleniDTO
    {
        public int Id { get; set; }
        public string ImePrezime { get; set; }
        public string Tip { get; set; } // "G", "R" ili "K"

        public int PozoristeId { get; set; } 
        public string NazivPozorista { get; set; } 
    }
}