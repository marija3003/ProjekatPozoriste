namespace ProjekatPozoriste.DTOs
{
    //za vizuelni prikaz sale (svako sjediste je jedan kartadto)
    public class KartaDTO
    {
        public int Id { get; set; }
        public double Cijena { get; set; }
        public int BrojSjedista { get; set; }
        public bool IsProdata { get; set; }
    }
}