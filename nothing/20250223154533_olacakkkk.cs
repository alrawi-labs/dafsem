using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class olacakkkk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KuralTuru_AltSayfa_SayfaId",
                table: "KuralTuru");

            migrationBuilder.AlterColumn<int>(
                name: "SayfaId",
                table: "KuralTuru",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "KuralTuru",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Kurallar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KuralTuru_DilId",
                table: "KuralTuru",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Kurallar_DilId",
                table: "Kurallar",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kurallar_Dil_DilId",
                table: "Kurallar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KuralTuru_AltSayfa_SayfaId",
                table: "KuralTuru",
                column: "SayfaId",
                principalTable: "AltSayfa",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KuralTuru_Dil_DilId",
                table: "KuralTuru",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kurallar_Dil_DilId",
                table: "Kurallar");

            migrationBuilder.DropForeignKey(
                name: "FK_KuralTuru_AltSayfa_SayfaId",
                table: "KuralTuru");

            migrationBuilder.DropForeignKey(
                name: "FK_KuralTuru_Dil_DilId",
                table: "KuralTuru");

            migrationBuilder.DropIndex(
                name: "IX_KuralTuru_DilId",
                table: "KuralTuru");

            migrationBuilder.DropIndex(
                name: "IX_Kurallar_DilId",
                table: "Kurallar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "KuralTuru");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Kurallar");

            migrationBuilder.AlterColumn<int>(
                name: "SayfaId",
                table: "KuralTuru",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KuralTuru_AltSayfa_SayfaId",
                table: "KuralTuru",
                column: "SayfaId",
                principalTable: "AltSayfa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
