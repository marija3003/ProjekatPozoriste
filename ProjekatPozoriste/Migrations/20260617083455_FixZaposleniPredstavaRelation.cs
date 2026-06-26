using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPozoriste.Migrations
{
    /// <inheritdoc />
    public partial class FixZaposleniPredstavaRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zaposleni_Predstave_PredstavaId",
                table: "Zaposleni");

            migrationBuilder.DropIndex(
                name: "IX_Zaposleni_PredstavaId",
                table: "Zaposleni");

            migrationBuilder.DropColumn(
                name: "PredstavaId",
                table: "Zaposleni");

            migrationBuilder.CreateTable(
                name: "PredstavaZaposleni",
                columns: table => new
                {
                    PredstaveId = table.Column<int>(type: "int", nullable: false),
                    UcesniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredstavaZaposleni", x => new { x.PredstaveId, x.UcesniciId });
                    table.ForeignKey(
                        name: "FK_PredstavaZaposleni_Predstave_PredstaveId",
                        column: x => x.PredstaveId,
                        principalTable: "Predstave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredstavaZaposleni_Zaposleni_UcesniciId",
                        column: x => x.UcesniciId,
                        principalTable: "Zaposleni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PredstavaZaposleni_UcesniciId",
                table: "PredstavaZaposleni",
                column: "UcesniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredstavaZaposleni");

            migrationBuilder.AddColumn<int>(
                name: "PredstavaId",
                table: "Zaposleni",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_PredstavaId",
                table: "Zaposleni",
                column: "PredstavaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zaposleni_Predstave_PredstavaId",
                table: "Zaposleni",
                column: "PredstavaId",
                principalTable: "Predstave",
                principalColumn: "Id");
        }
    }
}
