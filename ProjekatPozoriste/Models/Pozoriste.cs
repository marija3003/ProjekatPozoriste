namespace ProjekatPozoriste.Models
{
    public class Pozoriste
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public List<Zaposleni> Zaposleni { get; set; } = [];
        public List<Predstava> Predstave { get; set; } = [];
    }
}
