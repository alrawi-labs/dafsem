using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class ucretlerdili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Ucretler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "ParaBirimleri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ucretler_DilId",
                table: "Ucretler",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_ParaBirimleri_DilId",
                table: "ParaBirimleri",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParaBirimleri_Dil_DilId",
                table: "ParaBirimleri",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ucretler_Dil_DilId",
                table: "Ucretler",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParaBirimleri_Dil_DilId",
                table: "ParaBirimleri");

            migrationBuilder.DropForeignKey(
                name: "FK_Ucretler_Dil_DilId",
                table: "Ucretler");

            migrationBuilder.DropIndex(
                name: "IX_Ucretler_DilId",
                table: "Ucretler");

            migrationBuilder.DropIndex(
                name: "IX_ParaBirimleri_DilId",
                table: "ParaBirimleri");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Ucretler");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "ParaBirimleri");
        }
    }
}
