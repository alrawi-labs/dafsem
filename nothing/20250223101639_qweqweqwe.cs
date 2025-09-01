using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class qweqweqwe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Basliklar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Basliklar_DilId",
                table: "Basliklar",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basliklar_Dil_DilId",
                table: "Basliklar",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basliklar_Dil_DilId",
                table: "Basliklar");

            migrationBuilder.DropIndex(
                name: "IX_Basliklar_DilId",
                table: "Basliklar");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Basliklar");
        }
    }
}
