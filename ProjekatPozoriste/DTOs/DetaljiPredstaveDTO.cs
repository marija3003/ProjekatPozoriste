namespace ProjekatPozoriste.DTOs
{
    // za detaljan prikaz jedne predstave 
    public class DetaljiPredstaveDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int PozoristeId { get; set; }
        public string NazivPozorista { get; set; }
        public List<ZaposleniDTO> Ucesnici { get; set; } = [];
    }
}

