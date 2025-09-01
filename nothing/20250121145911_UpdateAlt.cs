using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AltSayfa_Sayfalar_SayfalarId",
                table: "AltSayfa");

            migrationBuilder.DropIndex(
                name: "IX_AltSayfa_SayfalarId",
                table: "AltSayfa");

            migrationBuilder.DropColumn(
                name: "SayfalarId",
                table: "AltSayfa");

            migrationBuilder.DropColumn(
                name: "UstSayfa",
                table: "AltSayfa");

            migrationBuilder.AddColumn<int>(
                name: "UstSayfaId",
                table: "AltSayfa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AltSayfa_UstSayfaId",
                table: "AltSayfa",
                column: "UstSayfaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AltSayfa_Sayfalar_UstSayfaId",
                table: "AltSayfa",
                column: "UstSayfaId",
                principalTable: "Sayfalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AltSayfa_Sayfalar_UstSayfaId",
                table: "AltSayfa");

            migrationBuilder.DropIndex(
                name: "IX_AltSayfa_UstSayfaId",
                table: "AltSayfa");

            migrationBuilder.DropColumn(
                name: "UstSayfaId",
                table: "AltSayfa");

            migrationBuilder.AddColumn<int>(
                name: "SayfalarId",
                table: "AltSayfa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UstSayfa",
                table: "AltSayfa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AltSayfa_SayfalarId",
                table: "AltSayfa",
                column: "SayfalarId");

            migrationBuilder.AddForeignKey(
                name: "FK_AltSayfa_Sayfalar_SayfalarId",
                table: "AltSayfa",
                column: "SayfalarId",
                principalTable: "Sayfalar",
                principalColumn: "Id");
        }
    }
}
