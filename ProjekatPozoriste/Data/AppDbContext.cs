namespace ProjekatPozoriste.Data
{
    using Microsoft.EntityFrameworkCore;
    using ProjekatPozoriste.Models;

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Pozoriste> Pozorista { get; set; }
        public DbSet<Zaposleni> Zaposleni { get; set; }
        public DbSet<Predstava> Predstave { get; set; }
        public DbSet<Karta> Karte { get; set; }
        public DbSet<Termin> Termini { get; set; }
        public DbSet<ProdajaKarte> ProdajaKarata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
        }
    }
}
