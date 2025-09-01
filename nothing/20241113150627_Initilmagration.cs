using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class Initilmagration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FotoGruplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoGruplar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnaSayfa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AfiseIdId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnaSayfa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fotolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FotoGrupIdId = table.Column<int>(type: "int", nullable: false),
                    Yol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnaSayfaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotolar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotolar_AnaSayfa_AnaSayfaId",
                        column: x => x.AnaSayfaId,
                        principalTable: "AnaSayfa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fotolar_FotoGruplar_FotoGrupIdId",
                        column: x => x.FotoGrupIdId,
                        principalTable: "FotoGruplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnaSayfa_AfiseIdId",
                table: "AnaSayfa",
                column: "AfiseIdId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotolar_AnaSayfaId",
                table: "Fotolar",
                column: "AnaSayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotolar_FotoGrupIdId",
                table: "Fotolar",
                column: "FotoGrupIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnaSayfa_Fotolar_AfiseIdId",
                table: "AnaSayfa",
                column: "AfiseIdId",
                principalTable: "Fotolar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnaSayfa_Fotolar_AfiseIdId",
                table: "AnaSayfa");

            migrationBuilder.DropTable(
                name: "Fotolar");

            migrationBuilder.DropTable(
                name: "AnaSayfa");

            migrationBuilder.DropTable(
                name: "FotoGruplar");
        }
    }
}
