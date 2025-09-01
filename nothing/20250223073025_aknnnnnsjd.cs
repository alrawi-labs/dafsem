using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class aknnnnnsjd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DilAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DilKodu = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dil", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dil_DilKodu",
                table: "Dil",
                column: "DilKodu",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dil");
        }
    }
}
