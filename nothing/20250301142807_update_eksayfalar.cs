using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class update_eksayfalar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SayfaBilesen_Bilesenler_BilesenId",
                table: "SayfaBilesen");

            migrationBuilder.DropTable(
                name: "Bilesenler");

            migrationBuilder.DropIndex(
                name: "IX_SayfaBilesen_BilesenId",
                table: "SayfaBilesen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bilesenler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilesenler", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SayfaBilesen_BilesenId",
                table: "SayfaBilesen",
                column: "BilesenId");

            migrationBuilder.AddForeignKey(
                name: "FK_SayfaBilesen_Bilesenler_BilesenId",
                table: "SayfaBilesen",
                column: "BilesenId",
                principalTable: "Bilesenler",
                principalColumn: "Id");
        }
    }
}
