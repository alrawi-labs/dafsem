using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class AnaSayfaDilTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Fotolar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "AnaSayfa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fotolar_DilId",
                table: "Fotolar",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_AnaSayfa_DilId",
                table: "AnaSayfa",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnaSayfa_Dil_DilId",
                table: "AnaSayfa",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotolar_Dil_DilId",
                table: "Fotolar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnaSayfa_Dil_DilId",
                table: "AnaSayfa");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotolar_Dil_DilId",
                table: "Fotolar");

            migrationBuilder.DropIndex(
                name: "IX_Fotolar_DilId",
                table: "Fotolar");

            migrationBuilder.DropIndex(
                name: "IX_AnaSayfa_DilId",
                table: "AnaSayfa");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Fotolar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "AnaSayfa");
        }
    }
}
