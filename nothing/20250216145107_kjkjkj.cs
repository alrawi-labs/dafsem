using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class kjkjkj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefonlar_Iletisim_IletisimId",
                table: "Telefonlar");

            migrationBuilder.AlterColumn<int>(
                name: "IletisimId",
                table: "Telefonlar",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Telefonlar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Iletisim",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonlar_Iletisim_IletisimId",
                table: "Telefonlar",
                column: "IletisimId",
                principalTable: "Iletisim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefonlar_Iletisim_IletisimId",
                table: "Telefonlar");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Telefonlar");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Iletisim");

            migrationBuilder.AlterColumn<int>(
                name: "IletisimId",
                table: "Telefonlar",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonlar_Iletisim_IletisimId",
                table: "Telefonlar",
                column: "IletisimId",
                principalTable: "Iletisim",
                principalColumn: "Id");
        }
    }
}
