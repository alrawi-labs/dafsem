using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class uyelerKategorilerUnvanlar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "Unvan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "KurulUyeleri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DilId",
                table: "KurulKategorileri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unvan_DilId",
                table: "Unvan",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_KurulUyeleri_DilId",
                table: "KurulUyeleri",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_KurulKategorileri_DilId",
                table: "KurulKategorileri",
                column: "DilId");

            migrationBuilder.AddForeignKey(
                name: "FK_KurulKategorileri_Dil_DilId",
                table: "KurulKategorileri",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KurulUyeleri_Dil_DilId",
                table: "KurulUyeleri",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Unvan_Dil_DilId",
                table: "Unvan",
                column: "DilId",
                principalTable: "Dil",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KurulKategorileri_Dil_DilId",
                table: "KurulKategorileri");

            migrationBuilder.DropForeignKey(
                name: "FK_KurulUyeleri_Dil_DilId",
                table: "KurulUyeleri");

            migrationBuilder.DropForeignKey(
                name: "FK_Unvan_Dil_DilId",
                table: "Unvan");

            migrationBuilder.DropIndex(
                name: "IX_Unvan_DilId",
                table: "Unvan");

            migrationBuilder.DropIndex(
                name: "IX_KurulUyeleri_DilId",
                table: "KurulUyeleri");

            migrationBuilder.DropIndex(
                name: "IX_KurulKategorileri_DilId",
                table: "KurulKategorileri");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "Unvan");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "KurulUyeleri");

            migrationBuilder.DropColumn(
                name: "DilId",
                table: "KurulKategorileri");
        }
    }
}
