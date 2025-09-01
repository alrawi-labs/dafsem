using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class kj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unvan_Sira",
                table: "Unvan");

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "ParaBirimleri",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "ParaBirimleri");

            migrationBuilder.CreateIndex(
                name: "IX_Unvan_Sira",
                table: "Unvan",
                column: "Sira",
                unique: true);
        }
    }
}
