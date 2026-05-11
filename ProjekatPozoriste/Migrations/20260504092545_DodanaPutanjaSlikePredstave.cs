using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPozoriste.Migrations
{
    /// <inheritdoc />
    public partial class DodanaPutanjaSlikePredstave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PutanjaSlike",
                table: "Predstave",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PutanjaSlike",
                table: "Predstave");
        }
    }
}
