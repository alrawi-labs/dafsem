using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKatilimciTuruToBaslikInUcretler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KatilimciTuru",
                table: "Ucretler",
                newName: "Baslik");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Baslik",
                table: "Ucretler",
                newName: "KatilimciTuru");
        }
    }
}
