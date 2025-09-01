using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class IletisimDilTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Telefonlar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Iletisim",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Telefonlar_DilId",
                table: "Telefonlar",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Iletisim_DilId",
                table: "Iletisim",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Iletisim_Dil_DilId",
                table: "Iletisim",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonlar_Dil_DilId",
                table: "Telefonlar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iletisim_Dil_DilId",
                table: "Iletisim");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefonlar_Dil_DilId",
                table: "Telefonlar");

            migrationBuilder.DropIndex(
                name: "IX_Telefonlar_DilId",
                table: "Telefonlar");

            migrationBuilder.DropIndex(
                name: "IX_Iletisim_DilId",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Telefonlar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Iletisim");
        }
    }
}
