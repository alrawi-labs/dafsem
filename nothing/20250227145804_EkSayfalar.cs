using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class EkSayfalar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "EkSayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SayfaBasligi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BulunduguSayfaId = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EkSayfalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EkSayfalar_EkSayfalar_BulunduguSayfaId",
                        column: x => x.BulunduguSayfaId,
                        principalTable: "EkSayfalar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SayfaBilesen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BilesenId = table.Column<int>(type: "int", nullable: true),
                    EkSayfaId = table.Column<int>(type: "int", nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaBilesen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaBilesen_Bilesenler_BilesenId",
                        column: x => x.BilesenId,
                        principalTable: "Bilesenler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SayfaBilesen_EkSayfalar_EkSayfaId",
                        column: x => x.EkSayfaId,
                        principalTable: "EkSayfalar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SayfaBilesenDegerleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SayfaBilesenId = table.Column<int>(type: "int", nullable: true),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaBilesenDegerleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaBilesenDegerleri_SayfaBilesen_SayfaBilesenId",
                        column: x => x.SayfaBilesenId,
                        principalTable: "SayfaBilesen",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EkSayfalar_BulunduguSayfaId",
                table: "EkSayfalar",
                column: "BulunduguSayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaBilesen_BilesenId",
                table: "SayfaBilesen",
                column: "BilesenId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaBilesen_EkSayfaId",
                table: "SayfaBilesen",
                column: "EkSayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaBilesenDegerleri_SayfaBilesenId",
                table: "SayfaBilesenDegerleri",
                column: "SayfaBilesenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SayfaBilesenDegerleri");

            migrationBuilder.DropTable(
                name: "SayfaBilesen");

            migrationBuilder.DropTable(
                name: "Bilesenler");

            migrationBuilder.DropTable(
                name: "EkSayfalar");
        }
    }
}
