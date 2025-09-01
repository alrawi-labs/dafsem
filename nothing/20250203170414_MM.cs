using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class MM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SayfaId",
                table: "KurulKategorileri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KurulKategorileri_SayfaId",
                table: "KurulKategorileri",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_KurulKategorileri_Sira",
                table: "KurulKategorileri",
                column: "Sira",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KurulKategorileri_AltSayfa_SayfaId",
                table: "KurulKategorileri",
                column: "SayfaId",
                principalTable: "AltSayfa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KurulKategorileri_AltSayfa_SayfaId",
                table: "KurulKategorileri");

            migrationBuilder.DropIndex(
                name: "IX_KurulKategorileri_SayfaId",
                table: "KurulKategorileri");

            migrationBuilder.DropIndex(
                name: "IX_KurulKategorileri_Sira",
                table: "KurulKategorileri");

            migrationBuilder.DropColumn(
                name: "SayfaId",
                table: "KurulKategorileri");
        }
    }
}
