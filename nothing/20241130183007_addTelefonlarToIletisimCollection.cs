using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class addTelefonlarToIletisimCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iletisim_Telefonlar_TelefonId",
                table: "Iletisim");

            migrationBuilder.DropIndex(
                name: "IX_Iletisim_TelefonId",
                table: "Iletisim");

            migrationBuilder.DropColumn(
                name: "TelefonId",
                table: "Iletisim");

            migrationBuilder.AddColumn<int>(
                name: "IletisimId",
                table: "Telefonlar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Telefonlar_IletisimId",
                table: "Telefonlar",
                column: "IletisimId");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonlar_Iletisim_IletisimId",
                table: "Telefonlar",
                column: "IletisimId",
                principalTable: "Iletisim",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefonlar_Iletisim_IletisimId",
                table: "Telefonlar");

            migrationBuilder.DropIndex(
                name: "IX_Telefonlar_IletisimId",
                table: "Telefonlar");

            migrationBuilder.DropColumn(
                name: "IletisimId",
                table: "Telefonlar");

            migrationBuilder.AddColumn<int>(
                name: "TelefonId",
                table: "Iletisim",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Iletisim_TelefonId",
                table: "Iletisim",
                column: "TelefonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Iletisim_Telefonlar_TelefonId",
                table: "Iletisim",
                column: "TelefonId",
                principalTable: "Telefonlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
