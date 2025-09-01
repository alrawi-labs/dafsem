using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class bankabilgileirdili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "BankaBilgileri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankaBilgileri_DilId",
                table: "BankaBilgileri",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankaBilgileri_Dil_DilId",
                table: "BankaBilgileri",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankaBilgileri_Dil_DilId",
                table: "BankaBilgileri");

            migrationBuilder.DropIndex(
                name: "IX_BankaBilgileri_DilId",
                table: "BankaBilgileri");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "BankaBilgileri");
        }
    }
}
