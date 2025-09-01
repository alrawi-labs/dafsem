using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKondaklamaToKonaklama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OdaTipleri_Kondaklama_KonaklamaEviId",
                table: "OdaTipleri");

            migrationBuilder.DropForeignKey(
                name: "FK_OdaTipleri_ParaBirimi_BirimId",
                table: "OdaTipleri");

            migrationBuilder.DropForeignKey(
                name: "FK_Ucretler_ParaBirimi_BirimId",
                table: "Ucretler");

            migrationBuilder.DropTable(
                name: "Kondaklama");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParaBirimi",
                table: "ParaBirimi");

            migrationBuilder.RenameTable(
                name: "ParaBirimi",
                newName: "ParaBirimleri");

            migrationBuilder.AddColumn<string>(
                name: "Form",
                table: "Basvuru",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParaBirimleri",
                table: "ParaBirimleri",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Konaklama",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YerAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebSitesi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    YildizSayisi = table.Column<int>(type: "int", nullable: true),
                    KahvaltiDahilMi = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konaklama", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OdaTipleri_Konaklama_KonaklamaEviId",
                table: "OdaTipleri",
                column: "KonaklamaEviId",
                principalTable: "Konaklama",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OdaTipleri_ParaBirimleri_BirimId",
                table: "OdaTipleri",
                column: "BirimId",
                principalTable: "ParaBirimleri",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ucretler_ParaBirimleri_BirimId",
                table: "Ucretler",
                column: "BirimId",
                principalTable: "ParaBirimleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OdaTipleri_Konaklama_KonaklamaEviId",
                table: "OdaTipleri");

            migrationBuilder.DropForeignKey(
                name: "FK_OdaTipleri_ParaBirimleri_BirimId",
                table: "OdaTipleri");

            migrationBuilder.DropForeignKey(
                name: "FK_Ucretler_ParaBirimleri_BirimId",
                table: "Ucretler");

            migrationBuilder.DropTable(
                name: "Konaklama");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParaBirimleri",
                table: "ParaBirimleri");

            migrationBuilder.DropColumn(
                name: "Form",
                table: "Basvuru");

            migrationBuilder.RenameTable(
                name: "ParaBirimleri",
                newName: "ParaBirimi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParaBirimi",
                table: "ParaBirimi",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Kondaklama",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    KahvaltiDahilMi = table.Column<bool>(type: "bit", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WebSitesi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    YerAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    YildizSayisi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kondaklama", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OdaTipleri_Kondaklama_KonaklamaEviId",
                table: "OdaTipleri",
                column: "KonaklamaEviId",
                principalTable: "Kondaklama",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OdaTipleri_ParaBirimi_BirimId",
                table: "OdaTipleri",
                column: "BirimId",
                principalTable: "ParaBirimi",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ucretler_ParaBirimi_BirimId",
                table: "Ucretler",
                column: "BirimId",
                principalTable: "ParaBirimi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
