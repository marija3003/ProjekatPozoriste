using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPozoriste.Migrations
{
    /// <inheritdoc />
    public partial class databaseupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Karte_Predstave_PredstavaId",
                table: "Karte");

            migrationBuilder.RenameColumn(
                name: "PredstavaId",
                table: "Karte",
                newName: "TerminId");

            migrationBuilder.RenameColumn(
                name: "BrojSedista",
                table: "Karte",
                newName: "BrojSjedista");

            migrationBuilder.RenameIndex(
                name: "IX_Karte_PredstavaId",
                table: "Karte",
                newName: "IX_Karte_TerminId");

            migrationBuilder.AddColumn<double>(
                name: "Cijena",
                table: "Karte",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "ProdajaKarata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KartaId = table.Column<int>(type: "int", nullable: false),
                    ImeKupca = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProdavacId = table.Column<int>(type: "int", nullable: false),
                    DatumKupovine = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdajaKarata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdajaKarata_Karte_KartaId",
                        column: x => x.KartaId,
                        principalTable: "Karte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdajaKarata_Zaposleni_ProdavacId",
                        column: x => x.ProdavacId,
                        principalTable: "Zaposleni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Termini",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PredstavaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termini", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Termini_Predstave_PredstavaId",
                        column: x => x.PredstavaId,
                        principalTable: "Predstave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProdajaKarata_KartaId",
                table: "ProdajaKarata",
                column: "KartaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdajaKarata_ProdavacId",
                table: "ProdajaKarata",
                column: "ProdavacId");

            migrationBuilder.CreateIndex(
                name: "IX_Termini_PredstavaId",
                table: "Termini",
                column: "PredstavaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Karte_Termini_TerminId",
                table: "Karte",
                column: "TerminId",
                principalTable: "Termini",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Karte_Termini_TerminId",
                table: "Karte");

            migrationBuilder.DropTable(
                name: "ProdajaKarata");

            migrationBuilder.DropTable(
                name: "Termini");

            migrationBuilder.DropColumn(
                name: "Cijena",
                table: "Karte");

            migrationBuilder.RenameColumn(
                name: "TerminId",
                table: "Karte",
                newName: "PredstavaId");

            migrationBuilder.RenameColumn(
                name: "BrojSjedista",
                table: "Karte",
                newName: "BrojSedista");

            migrationBuilder.RenameIndex(
                name: "IX_Karte_TerminId",
                table: "Karte",
                newName: "IX_Karte_PredstavaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Karte_Predstave_PredstavaId",
                table: "Karte",
                column: "PredstavaId",
                principalTable: "Predstave",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
