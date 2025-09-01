using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCuretler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BirimId",
                table: "Ucretler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ParaBirimi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sembol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParaBirimi", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ucretler_BirimId",
                table: "Ucretler",
                column: "BirimId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ucretler_ParaBirimi_BirimId",
                table: "Ucretler",
                column: "BirimId",
                principalTable: "ParaBirimi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ucretler_ParaBirimi_BirimId",
                table: "Ucretler");

            migrationBuilder.DropTable(
                name: "ParaBirimi");

            migrationBuilder.DropIndex(
                name: "IX_Ucretler_BirimId",
                table: "Ucretler");

            migrationBuilder.DropColumn(
                name: "BirimId",
                table: "Ucretler");
        }
    }
}
