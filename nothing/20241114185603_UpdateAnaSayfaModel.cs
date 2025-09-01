using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnaSayfaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnaSayfa_Fotolar_AfiseIdId",
                table: "AnaSayfa");

            migrationBuilder.AlterColumn<int>(
                name: "AfiseIdId",
                table: "AnaSayfa",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Mektup",
                table: "AnaSayfa",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnaSayfa_Fotolar_AfiseIdId",
                table: "AnaSayfa",
                column: "AfiseIdId",
                principalTable: "Fotolar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnaSayfa_Fotolar_AfiseIdId",
                table: "AnaSayfa");

            migrationBuilder.DropColumn(
                name: "Mektup",
                table: "AnaSayfa");

            migrationBuilder.AlterColumn<int>(
                name: "AfiseIdId",
                table: "AnaSayfa",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnaSayfa_Fotolar_AfiseIdId",
                table: "AnaSayfa",
                column: "AfiseIdId",
                principalTable: "Fotolar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
