namespace ProjekatPozoriste.Models
{
    public class Karta
    {
        public int Id { get; set; }
        public int BrojSedista { get; set; }
        public bool IsProdata { get; set; }

        public int PredstavaId { get; set; }
        public Predstava Predstava { get; set; }
    }
}
