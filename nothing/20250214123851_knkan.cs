using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class knkan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hizmetler_HizmetTuru_TuruId",
                table: "Hizmetler");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Hizmetler",
                newName: "Hizmetler",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "TuruId",
                schema: "dbo",
                table: "Hizmetler",
                newName: "HizmetTuruId");

            migrationBuilder.RenameIndex(
                name: "IX_Hizmetler_TuruId",
                schema: "dbo",
                table: "Hizmetler",
                newName: "IX_Hizmetler_HizmetTuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hizmetler_HizmetTuru_HizmetTuruId",
                schema: "dbo",
                table: "Hizmetler",
                column: "HizmetTuruId",
                principalTable: "HizmetTuru",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hizmetler_HizmetTuru_HizmetTuruId",
                schema: "dbo",
                table: "Hizmetler");

            migrationBuilder.RenameTable(
                name: "Hizmetler",
                schema: "dbo",
                newName: "Hizmetler");

            migrationBuilder.RenameColumn(
                name: "HizmetTuruId",
                table: "Hizmetler",
                newName: "TuruId");

            migrationBuilder.RenameIndex(
                name: "IX_Hizmetler_HizmetTuruId",
                table: "Hizmetler",
                newName: "IX_Hizmetler_TuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hizmetler_HizmetTuru_TuruId",
                table: "Hizmetler",
                column: "TuruId",
                principalTable: "HizmetTuru",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
