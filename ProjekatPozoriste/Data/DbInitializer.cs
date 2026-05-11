using ProjekatPozoriste.Enums;
using ProjekatPozoriste.Models;

namespace ProjekatPozoriste.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // POZORISTA
            if (!context.Pozorista.Any())
            {
                var pozorista = new Pozoriste[]
                {
                new() { Naziv = "Narodno pozorište" },
                new() { Naziv = "Gradsko pozorište Jazavac" }
                };

                context.Pozorista.AddRange(pozorista);
                context.SaveChanges();
            }

            // ZAPOSLENI
            if (!context.Zaposleni.Any())
            {
                var zaposleni = new Zaposleni[]
                {
                new() { Ime = "Marko", Prezime = "Markovic", Tip = TipZaposlenog.R, PozoristeId = 1 },
                new() { Ime = "Jovana", Prezime = "Jovanovic", Tip = TipZaposlenog.G, PozoristeId = 1 },
                new() { Ime = "Nikola", Prezime = "Nikolic", Tip = TipZaposlenog.G, PozoristeId = 1 },
                new() { Ime = "Ana", Prezime = "Dragic", Tip = TipZaposlenog.K, PozoristeId = 1 },

                new() { Ime = "Petar", Prezime = "Petrovic", Tip = TipZaposlenog.G, PozoristeId = 2 },
                new() { Ime = "Dragan", Prezime = "Dragic", Tip = TipZaposlenog.R, PozoristeId = 2 }
                };

                context.Zaposleni.AddRange(zaposleni);
                context.SaveChanges();
            }

            // PREDSTAVE
            if (!context.Predstave.Any())
            {
                var predstave = new Predstava[]
                {
                new()
                {
                    Naziv = "Predstava 1",
                    PozoristeId = 1,
                    PutanjaSlike = "https://static.ffx.io/images/$zoom_0.369140625%2C$multiply_0.7725%2C$ratio_1.5%2C$width_756%2C$x_0%2C$y_0/t_crop_custom/q_86%2Cf_auto/45945ae17062bbcc507a504a27cedbf2b22ab8e1"
                }
                };

                context.Predstave.AddRange(predstave);
                context.SaveChanges();
            }

            // TERMINI
            if (!context.Termini.Any())
            {
                var termini = new Termin[]
                {
                new() { PredstavaId = 1, DatumVrijeme = DateTime.Now },
                new() { PredstavaId = 1, DatumVrijeme = DateTime.Now.AddDays(1) }
                };

                context.Termini.AddRange(termini);
                context.SaveChanges();
            }
        }
    }
}