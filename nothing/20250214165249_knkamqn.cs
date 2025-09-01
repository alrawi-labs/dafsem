using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class knkamqn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kurallar_KuralTuru_TuruId",
                table: "Kurallar");

            migrationBuilder.RenameTable(
                name: "Hizmetler",
                schema: "dbo",
                newName: "Hizmetler");

            migrationBuilder.RenameColumn(
                name: "TuruId",
                table: "Kurallar",
                newName: "KuralTuruId");

            migrationBuilder.RenameIndex(
                name: "IX_Kurallar_TuruId",
                table: "Kurallar",
                newName: "IX_Kurallar_KuralTuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kurallar_KuralTuru_KuralTuruId",
                table: "Kurallar",
                column: "KuralTuruId",
                principalTable: "KuralTuru",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kurallar_KuralTuru_KuralTuruId",
                table: "Kurallar");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Hizmetler",
                newName: "Hizmetler",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "KuralTuruId",
                table: "Kurallar",
                newName: "TuruId");

            migrationBuilder.RenameIndex(
                name: "IX_Kurallar_KuralTuruId",
                table: "Kurallar",
                newName: "IX_Kurallar_TuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kurallar_KuralTuru_TuruId",
                table: "Kurallar",
                column: "TuruId",
                principalTable: "KuralTuru",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
