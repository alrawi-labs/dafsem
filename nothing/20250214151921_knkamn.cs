using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class knkamn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "KuralTuru",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Kurallar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "AltSayfa",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "KuralTuru");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Kurallar");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AltSayfa");
        }
    }
}
