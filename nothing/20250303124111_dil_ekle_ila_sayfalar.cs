using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class dil_ekle_ila_sayfalar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "SayfaBilesenDegerleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "SayfaBilesen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "EkSayfalar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SayfaBilesenDegerleri_DilId",
                table: "SayfaBilesenDegerleri",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaBilesen_DilId",
                table: "SayfaBilesen",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_EkSayfalar_DilId",
                table: "EkSayfalar",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_EkSayfalar_Dil_DilId",
                table: "EkSayfalar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SayfaBilesen_Dil_DilId",
                table: "SayfaBilesen",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SayfaBilesenDegerleri_Dil_DilId",
                table: "SayfaBilesenDegerleri",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EkSayfalar_Dil_DilId",
                table: "EkSayfalar");

            migrationBuilder.DropForeignKey(
                name: "FK_SayfaBilesen_Dil_DilId",
                table: "SayfaBilesen");

            migrationBuilder.DropForeignKey(
                name: "FK_SayfaBilesenDegerleri_Dil_DilId",
                table: "SayfaBilesenDegerleri");

            migrationBuilder.DropIndex(
                name: "IX_SayfaBilesenDegerleri_DilId",
                table: "SayfaBilesenDegerleri");

            migrationBuilder.DropIndex(
                name: "IX_SayfaBilesen_DilId",
                table: "SayfaBilesen");

            migrationBuilder.DropIndex(
                name: "IX_EkSayfalar_DilId",
                table: "EkSayfalar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "SayfaBilesenDegerleri");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "SayfaBilesen");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "EkSayfalar");
        }
    }
}
