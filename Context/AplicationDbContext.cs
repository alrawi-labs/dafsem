using dafsem.Models;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Context
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }

        public DbSet<AltSayfa> AltSayfa { get; set; }
        public DbSet<AnaSayfa> AnaSayfa { get; set; }
        public DbSet<Ayarlar> Ayarlar { get; set; }
        public DbSet<BankaBilgileri> BankaBilgileri { get; set; }
        public DbSet<Basliklar> Basliklar { get; set; }
        public DbSet<Basvuru> Basvuru { get; set; }
        public DbSet<Fotolar> Fotolar { get; set; }
        public DbSet<Hizmetler> Hizmetler { get; set; }
        public DbSet<HizmetTuru> HizmetTuru { get; set; }
        public DbSet<Konaklama> Konaklama { get; set; }
        public DbSet<Kurallar> Kurallar { get; set; }
        public DbSet<KuralTuru> KuralTuru { get; set; }
        public DbSet<KurulKategorileri> KurulKategorileri { get; set; }
        public DbSet<KurulUyeleri> KurulUyeleri { get; set; }
        public DbSet<Unvan> Unvan { get; set; }
        public DbSet<Sayfalar> Sayfalar { get; set; }
        public DbSet<Tarihler> Tarihler { get; set; }
        public DbSet<Telefonlar> Telefonlar { get; set; }
        public DbSet<Ucretler> Ucretler { get; set; }
        public DbSet<ParaBirimi> ParaBirimi { get; set; }
        public DbSet<OdaTipleri> OdaTipleri { get; set; }
        public DbSet<Iletisim> Iletisim { get; set; }
        public DbSet<Dil> Dil { get; set; }
        public DbSet<EkSayfalar> EkSayfalar { get; set; }
        public DbSet<SayfaBilesen> SayfaBilesen { get; set; }
        public DbSet<SayfaBilesenDegerleri> SayfaBilesenDegerleri { get; set; }

    }
}
