using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class BasvuruDiliTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Basvuru",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Basvuru_DilId",
                table: "Basvuru",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basvuru_Dil_DilId",
                table: "Basvuru",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basvuru_Dil_DilId",
                table: "Basvuru");

            migrationBuilder.DropIndex(
                name: "IX_Basvuru_DilId",
                table: "Basvuru");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Basvuru");
        }
    }
}
