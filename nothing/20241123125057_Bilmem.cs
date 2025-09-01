using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class Bilmem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefonlar_Ayarlar_AyarlarId",
                table: "Telefonlar");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Ayarlar");

            migrationBuilder.RenameColumn(
                name: "AyarlarId",
                table: "Telefonlar",
                newName: "IletisimId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefonlar_AyarlarId",
                table: "Telefonlar",
                newName: "IX_Telefonlar_IletisimId");

            migrationBuilder.CreateTable(
                name: "Iletisim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Eposta = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iletisim", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "Iletisim");

            migrationBuilder.RenameColumn(
                name: "IletisimId",
                table: "Telefonlar",
                newName: "AyarlarId");

            migrationBuilder.RenameIndex(
                name: "IX_Telefonlar_IletisimId",
                table: "Telefonlar",
                newName: "IX_Telefonlar_AyarlarId");

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Ayarlar",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonlar_Ayarlar_AyarlarId",
                table: "Telefonlar",
                column: "AyarlarId",
                principalTable: "Ayarlar",
                principalColumn: "Id");
        }
    }
}
