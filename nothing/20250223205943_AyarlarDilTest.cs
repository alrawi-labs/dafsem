using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class AyarlarDilTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Sayfalar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Ayarlar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "AltSayfa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sayfalar_DilId",
                table: "Sayfalar",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_DilId",
                table: "Ayarlar",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_AltSayfa_DilId",
                table: "AltSayfa",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_AltSayfa_Dil_DilId",
                table: "AltSayfa",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ayarlar_Dil_DilId",
                table: "Ayarlar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sayfalar_Dil_DilId",
                table: "Sayfalar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AltSayfa_Dil_DilId",
                table: "AltSayfa");

            migrationBuilder.DropForeignKey(
                name: "FK_Ayarlar_Dil_DilId",
                table: "Ayarlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Sayfalar_Dil_DilId",
                table: "Sayfalar");

            migrationBuilder.DropIndex(
                name: "IX_Sayfalar_DilId",
                table: "Sayfalar");

            migrationBuilder.DropIndex(
                name: "IX_Ayarlar_DilId",
                table: "Ayarlar");

            migrationBuilder.DropIndex(
                name: "IX_AltSayfa_DilId",
                table: "AltSayfa");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Sayfalar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Ayarlar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "AltSayfa");
        }
    }
}
