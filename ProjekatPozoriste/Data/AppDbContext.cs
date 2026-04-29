namespace ProjekatPozoriste.Data
{
    using Microsoft.EntityFrameworkCore;
    using ProjekatPozoriste.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pozoriste> Pozorista { get; set; }
        public DbSet<Zaposleni> Zaposleni { get; set; }
        public DbSet<Predstava> Predstave { get; set; }
        public DbSet<Karta> Karte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ovde dodatno konfigurisati relacije ako zatreba
            base.OnModelCreating(modelBuilder);
        }
    }
}
