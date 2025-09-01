using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class qweqaaw1eqwe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Tarihler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tarihler_DilId",
                table: "Tarihler",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarihler_Dil_DilId",
                table: "Tarihler",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tarihler_Dil_DilId",
                table: "Tarihler");

            migrationBuilder.DropIndex(
                name: "IX_Tarihler_DilId",
                table: "Tarihler");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Tarihler");
        }
    }
}
