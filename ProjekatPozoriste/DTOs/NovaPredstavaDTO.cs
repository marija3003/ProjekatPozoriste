namespace ProjekatPozoriste.DTOs
{
    //sa fronta, kad se popuni forma slanje apiju da se napravi novi zapis u bazi
    public class NovaPredstavaDTO
    {
        public string Naziv { get; set; }
        public int PozoristeId { get; set; }
        public IFormFile Slika { get; set; }
    }
}