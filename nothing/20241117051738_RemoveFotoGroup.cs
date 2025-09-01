using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFotoGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotolar_FotoGruplar_FotoGrupIdId",
                table: "Fotolar");

            migrationBuilder.DropTable(
                name: "FotoGruplar");

            migrationBuilder.DropIndex(
                name: "IX_Fotolar_FotoGrupIdId",
                table: "Fotolar");

            migrationBuilder.DropColumn(
                name: "FotoGrupIdId",
                table: "Fotolar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FotoGrupIdId",
                table: "Fotolar",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Fotolar_FotoGrupIdId",
                table: "Fotolar",
                column: "FotoGrupIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotolar_FotoGruplar_FotoGrupIdId",
                table: "Fotolar",
                column: "FotoGrupIdId",
                principalTable: "FotoGruplar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
