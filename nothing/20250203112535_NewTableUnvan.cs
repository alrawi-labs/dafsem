using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class NewTableUnvan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adminler");

            migrationBuilder.DropColumn(
                name: "Unvan",
                table: "KurulUyeleri");

            migrationBuilder.AddColumn<int>(
                name: "UnvanId",
                table: "KurulUyeleri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Unvan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnvanAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unvan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KurulUyeleri_UnvanId",
                table: "KurulUyeleri",
                column: "UnvanId");

            migrationBuilder.AddForeignKey(
                name: "FK_KurulUyeleri_Unvan_UnvanId",
                table: "KurulUyeleri",
                column: "UnvanId",
                principalTable: "Unvan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KurulUyeleri_Unvan_UnvanId",
                table: "KurulUyeleri");

            migrationBuilder.DropTable(
                name: "Unvan");

            migrationBuilder.DropIndex(
                name: "IX_KurulUyeleri_UnvanId",
                table: "KurulUyeleri");

            migrationBuilder.DropColumn(
                name: "UnvanId",
                table: "KurulUyeleri");

            migrationBuilder.AddColumn<string>(
                name: "Unvan",
                table: "KurulUyeleri",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Adminler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adminler", x => x.Id);
                });
        }
    }
}
