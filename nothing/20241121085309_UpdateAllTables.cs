using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dafsem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ayarlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KurumAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SiteLogoId = table.Column<int>(type: "int", nullable: true),
                    SagLogoId = table.Column<int>(type: "int", nullable: true),
                    SolLogoId = table.Column<int>(type: "int", nullable: true),
                    SiteAltBaslik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FiligranId = table.Column<int>(type: "int", nullable: true),
                    SiteArkaplaniId = table.Column<int>(type: "int", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    Program = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ayarlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ayarlar_Fotolar_FiligranId",
                        column: x => x.FiligranId,
                        principalTable: "Fotolar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ayarlar_Fotolar_SagLogoId",
                        column: x => x.SagLogoId,
                        principalTable: "Fotolar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ayarlar_Fotolar_SiteArkaplaniId",
                        column: x => x.SiteArkaplaniId,
                        principalTable: "Fotolar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ayarlar_Fotolar_SiteLogoId",
                        column: x => x.SiteLogoId,
                        principalTable: "Fotolar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ayarlar_Fotolar_SolLogoId",
                        column: x => x.SolLogoId,
                        principalTable: "Fotolar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankaBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankaAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HesapSahibiAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankaBilgileri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Basliklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basliklar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Basvuru",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UstMetin = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    AltMetin = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basvuru", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HizmetTuru",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tur = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HizmetTuru", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kondaklama",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YerAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WebSitesi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    YildizSayisi = table.Column<int>(type: "int", nullable: true),
                    KahvaltiDahilMi = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kondaklama", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KuralTuru",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tur = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KuralTuru", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarihler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarihler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ucretler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KatilimciTuru = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ucret = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ucretler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SayfaBasligi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AyarlarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayfalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sayfalar_Ayarlar_AyarlarId",
                        column: x => x.AyarlarId,
                        principalTable: "Ayarlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Telefonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Dahili = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AyarlarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefonlar_Ayarlar_AyarlarId",
                        column: x => x.AyarlarId,
                        principalTable: "Ayarlar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hizmet = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    TuruId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hizmetler_HizmetTuru_TuruId",
                        column: x => x.TuruId,
                        principalTable: "HizmetTuru",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kurallar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Metin = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TuruId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kurallar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kurallar_KuralTuru_TuruId",
                        column: x => x.TuruId,
                        principalTable: "KuralTuru",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AltSayfa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AltSayfaBaslik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UstSayfa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SayfalarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltSayfa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AltSayfa_Sayfalar_SayfalarId",
                        column: x => x.SayfalarId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AltSayfa_SayfalarId",
                table: "AltSayfa",
                column: "SayfalarId");

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_FiligranId",
                table: "Ayarlar",
                column: "FiligranId");

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_SagLogoId",
                table: "Ayarlar",
                column: "SagLogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_SiteArkaplaniId",
                table: "Ayarlar",
                column: "SiteArkaplaniId");

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_SiteLogoId",
                table: "Ayarlar",
                column: "SiteLogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ayarlar_SolLogoId",
                table: "Ayarlar",
                column: "SolLogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Hizmetler_TuruId",
                table: "Hizmetler",
                column: "TuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Kurallar_TuruId",
                table: "Kurallar",
                column: "TuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Sayfalar_AyarlarId",
                table: "Sayfalar",
                column: "AyarlarId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonlar_AyarlarId",
                table: "Telefonlar",
                column: "AyarlarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AltSayfa");

            migrationBuilder.DropTable(
                name: "BankaBilgileri");

            migrationBuilder.DropTable(
                name: "Basliklar");

            migrationBuilder.DropTable(
                name: "Basvuru");

            migrationBuilder.DropTable(
                name: "Hizmetler");

            migrationBuilder.DropTable(
                name: "Kondaklama");

            migrationBuilder.DropTable(
                name: "Kurallar");

            migrationBuilder.DropTable(
                name: "Tarihler");

            migrationBuilder.DropTable(
                name: "Telefonlar");

            migrationBuilder.DropTable(
                name: "Ucretler");

            migrationBuilder.DropTable(
                name: "Sayfalar");

            migrationBuilder.DropTable(
                name: "HizmetTuru");

            migrationBuilder.DropTable(
                name: "KuralTuru");

            migrationBuilder.DropTable(
                name: "Ayarlar");
        }
    }
}
