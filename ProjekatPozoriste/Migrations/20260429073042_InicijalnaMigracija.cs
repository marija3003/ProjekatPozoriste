using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPozoriste.Migrations
{
    /// <inheritdoc />
    public partial class InicijalnaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pozorista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pozorista", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Predstave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PozoristeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predstave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predstave_Pozorista_PozoristeId",
                        column: x => x.PozoristeId,
                        principalTable: "Pozorista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Karte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BrojSedista = table.Column<int>(type: "int", nullable: false),
                    IsProdata = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PredstavaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Karte_Predstave_PredstavaId",
                        column: x => x.PredstavaId,
                        principalTable: "Predstave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Zaposleni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prezime = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    PozoristeId = table.Column<int>(type: "int", nullable: false),
                    PredstavaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposleni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Pozorista_PozoristeId",
                        column: x => x.PozoristeId,
                        principalTable: "Pozorista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Predstave_PredstavaId",
                        column: x => x.PredstavaId,
                        principalTable: "Predstave",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Karte_PredstavaId",
                table: "Karte",
                column: "PredstavaId");

            migrationBuilder.CreateIndex(
                name: "IX_Predstave_PozoristeId",
                table: "Predstave",
                column: "PozoristeId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_PozoristeId",
                table: "Zaposleni",
                column: "PozoristeId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_PredstavaId",
                table: "Zaposleni",
                column: "PredstavaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Karte");

            migrationBuilder.DropTable(
                name: "Zaposleni");

            migrationBuilder.DropTable(
                name: "Predstave");

            migrationBuilder.DropTable(
                name: "Pozorista");
        }
    }
}
