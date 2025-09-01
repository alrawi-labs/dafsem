using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class konaklamadili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "OdaTipleri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Konaklama",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OdaTipleri_DilId",
                table: "OdaTipleri",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Konaklama_DilId",
                table: "Konaklama",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Konaklama_Dil_DilId",
                table: "Konaklama",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OdaTipleri_Dil_DilId",
                table: "OdaTipleri",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Konaklama_Dil_DilId",
                table: "Konaklama");

            migrationBuilder.DropForeignKey(
                name: "FK_OdaTipleri_Dil_DilId",
                table: "OdaTipleri");

            migrationBuilder.DropIndex(
                name: "IX_OdaTipleri_DilId",
                table: "OdaTipleri");

            migrationBuilder.DropIndex(
                name: "IX_Konaklama_DilId",
                table: "Konaklama");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "OdaTipleri");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Konaklama");
        }
    }
}
