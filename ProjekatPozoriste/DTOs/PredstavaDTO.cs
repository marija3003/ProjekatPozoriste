namespace ProjekatPozoriste.DTOs
{
    // grupiše ljude po ulogama, trebace za repertoar 
    public class PredstavaDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string PutanjaSlike { get; set; } 
        public string NazivPozorista { get; set; }
        public string Reditelj { get; set; }
        public List<string> Glumci { get; set; } = [];
        public List<string> Kostimografi { get; set; } = [];
    }
}