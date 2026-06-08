using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPozoriste.Migrations
{
    /// <inheritdoc />
    public partial class DodanStatusKarte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProdata",
                table: "Karte");

            migrationBuilder.AddColumn<int>(
                name: "Stanje",
                table: "Karte",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stanje",
                table: "Karte");

            migrationBuilder.AddColumn<bool>(
                name: "IsProdata",
                table: "Karte",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
