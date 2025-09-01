using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace dafsem.Services
{
    public class SayfaService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public SayfaService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task SayfalariSifirla()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            Ayarlar? ayarlar = await _context.Ayarlar
                .Include(s => s.Sayfalar.Where(s => s.State && s.DilId == dilId))
                .ThenInclude(a => a.AltSayfalari.Where(alt => alt.State && alt.DilId == dilId))
                .FirstOrDefaultAsync(a => a.State && a.DilId == dilId);

            await RemoveAllSayfalar(ayarlar!);
            await CheckAndCreateSayfalarAsync(ayarlar!);
        }
        private async Task RemoveAllSayfalar(Ayarlar ayarlar)
        {
            if (ayarlar == null || ayarlar.Sayfalar == null || !ayarlar.Sayfalar.Any())
                return;

            // Alt sayfaları sil
            foreach (var sayfa in ayarlar.Sayfalar)
            {
                if (sayfa.AltSayfalari != null && sayfa.AltSayfalari.Any())
                {
                    foreach (var alt in sayfa.AltSayfalari)
                    {
                        alt.State = false;
                    }
                }
                sayfa.State = false;
            }

            ayarlar.Sayfalar = null;
            _context.Update(ayarlar);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();
        }

        private async Task CheckAndCreateSayfalarAsync(Ayarlar ayarlar)
        {

            var yeniSayfalar = new List<Sayfalar>
            {
                new Sayfalar { SayfaBasligi = "Anasayfa", Url = "Home/Index", AyarlarId = ayarlar.Id, DilId = ayarlar.DilId },
                new Sayfalar { SayfaBasligi = "Kurullar", Url = "", AyarlarId = ayarlar.Id , DilId = ayarlar.DilId},
                new Sayfalar { SayfaBasligi = "Hakkında", Url = "", AyarlarId = ayarlar.Id , DilId = ayarlar.DilId},
                new Sayfalar { SayfaBasligi = "Başvuru", Url = "Home/Basvuru", AyarlarId = ayarlar.Id, DilId = ayarlar.DilId },
                new Sayfalar { SayfaBasligi = "İletişim", Url = "Home/Iletisim", AyarlarId = ayarlar.Id , DilId = ayarlar.DilId},
            };

            ayarlar.Sayfalar = yeniSayfalar;
            _context.Update(ayarlar);
            await _context.SaveChangesAsync();

            foreach (var sayfa in ayarlar.Sayfalar)
            {
                // Sadece "Kurullar" ve "Hakkında" başlıkları için alt sayfa oluştur
                if (sayfa.SayfaBasligi == "Kurullar" || sayfa.SayfaBasligi == "Hakkında")
                {
                    await CheckAndCreateAltSayfalarAsync(sayfa);
                }
            }
        }

        private async Task CheckAndCreateAltSayfalarAsync(Sayfalar sayfa)
        {
            if (sayfa.AltSayfalari == null || !sayfa.AltSayfalari.Any())
            {
                var altSayfalar = new List<AltSayfa>();

                if (sayfa.SayfaBasligi == "Kurullar")
                {
                    altSayfalar = new List<AltSayfa>
                    {
                        new AltSayfa { AltSayfaBaslik = "Düzenleme kurulu", UstSayfa = sayfa, Url="Home/DuzenlemeKurulu",DilId = sayfa.DilId },
                        new AltSayfa { AltSayfaBaslik = "Bilim Kurulu", UstSayfa = sayfa, Url="Home/BilimKurulu",DilId = sayfa.DilId }
                    };
                }
                else if (sayfa.SayfaBasligi == "Hakkında")
                {
                    altSayfalar = new List<AltSayfa>
                    {
                        new AltSayfa { AltSayfaBaslik = "Davetli Konuşmacılar", UstSayfa = sayfa,Url="Home/DavetliKonusmacilar",DilId = sayfa.DilId },
                        new AltSayfa { AltSayfaBaslik = "Başlıklar" , UstSayfa = sayfa,Url="Home/Basliklar",DilId = sayfa.DilId},
                        new AltSayfa { AltSayfaBaslik = "Program" , UstSayfa = sayfa,Url="Home/Program",DilId = sayfa.DilId},
                        new AltSayfa { AltSayfaBaslik = "Önemli Tarihler" , UstSayfa = sayfa,Url="Home/Tarihler",DilId = sayfa.DilId},
                        new AltSayfa { AltSayfaBaslik = "Yazım Kuralları", UstSayfa=sayfa,Url="Home/YazimKurallari",DilId = sayfa.DilId },
                        new AltSayfa { AltSayfaBaslik = "Sunum Kuralları" , UstSayfa = sayfa,Url="Home/SunumKurallari",DilId = sayfa.DilId},
                        new AltSayfa { AltSayfaBaslik = "Katılım Ücreti" , UstSayfa = sayfa,Url="Home/Ucretler",DilId = sayfa.DilId},
                        new AltSayfa { AltSayfaBaslik = "Konaklama" , UstSayfa = sayfa,Url="Home/Konaklama",DilId = sayfa.DilId}
                    };
                }

                sayfa.AltSayfalari = altSayfalar;
                _context.Update(sayfa);
                await _context.SaveChangesAsync();
            }
        }

    }
}
