using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class hizmetlerdili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "HizmetTuru",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Hizmetler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HizmetTuru_DilId",
                table: "HizmetTuru",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Hizmetler_DilId",
                table: "Hizmetler",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hizmetler_Dil_DilId",
                table: "Hizmetler",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HizmetTuru_Dil_DilId",
                table: "HizmetTuru",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hizmetler_Dil_DilId",
                table: "Hizmetler");

            migrationBuilder.DropForeignKey(
                name: "FK_HizmetTuru_Dil_DilId",
                table: "HizmetTuru");

            migrationBuilder.DropIndex(
                name: "IX_HizmetTuru_DilId",
                table: "HizmetTuru");

            migrationBuilder.DropIndex(
                name: "IX_Hizmetler_DilId",
                table: "Hizmetler");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "HizmetTuru");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Hizmetler");
        }
    }
}
