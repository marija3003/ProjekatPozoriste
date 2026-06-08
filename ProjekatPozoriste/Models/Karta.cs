using ProjekatPozoriste.Enums;

namespace ProjekatPozoriste.Models
{
    public class Karta
    {
        //vezati za termin , cijena dodati, ko je prodao kartu 
        public int Id { get; set; }
        public int BrojSjedista { get; set; } 
        public double Cijena { get; set; }
       // public bool IsProdata { get; set; }

        public StanjeKarte Stanje { get; set; }
      
        public int TerminId { get; set; }
        public Termin Termin { get; set; }
    }
}
