using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class AddOdaTipleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OdaTipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KonaklamaEviId = table.Column<int>(type: "int", nullable: false),
                    OdaTipi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ucret = table.Column<float>(type: "real", nullable: true),
                    BirimId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdaTipleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdaTipleri_Kondaklama_KonaklamaEviId",
                        column: x => x.KonaklamaEviId,
                        principalTable: "Kondaklama",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdaTipleri_ParaBirimi_BirimId",
                        column: x => x.BirimId,
                        principalTable: "ParaBirimi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdaTipleri_BirimId",
                table: "OdaTipleri",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_OdaTipleri_KonaklamaEviId",
                table: "OdaTipleri",
                column: "KonaklamaEviId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdaTipleri");
        }
    }
}
