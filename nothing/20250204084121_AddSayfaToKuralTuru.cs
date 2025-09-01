using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class AddSayfaToKuralTuru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SayfaId",
                table: "KuralTuru",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KuralTuru_SayfaId",
                table: "KuralTuru",
                column: "SayfaId");

            migrationBuilder.AddForeignKey(
                name: "FK_KuralTuru_AltSayfa_SayfaId",
                table: "KuralTuru",
                column: "SayfaId",
                principalTable: "AltSayfa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KuralTuru_AltSayfa_SayfaId",
                table: "KuralTuru");

            migrationBuilder.DropIndex(
                name: "IX_KuralTuru_SayfaId",
                table: "KuralTuru");

            migrationBuilder.DropColumn(
                name: "SayfaId",
                table: "KuralTuru");
        }
    }
}
