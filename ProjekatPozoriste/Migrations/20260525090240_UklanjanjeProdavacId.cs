using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPozoriste.Migrations
{
    /// <inheritdoc />
    public partial class UklanjanjeProdavacId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdajaKarata_Zaposleni_ProdavacId",
                table: "ProdajaKarata");

            migrationBuilder.AlterColumn<int>(
                name: "ProdavacId",
                table: "ProdajaKarata",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdajaKarata_Zaposleni_ProdavacId",
                table: "ProdajaKarata",
                column: "ProdavacId",
                principalTable: "Zaposleni",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdajaKarata_Zaposleni_ProdavacId",
                table: "ProdajaKarata");

            migrationBuilder.AlterColumn<int>(
                name: "ProdavacId",
                table: "ProdajaKarata",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdajaKarata_Zaposleni_ProdavacId",
                table: "ProdajaKarata",
                column: "ProdavacId",
                principalTable: "Zaposleni",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
