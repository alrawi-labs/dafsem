using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace dafsem.Services
{
    public class SayfalarService : ISayfalarService
    {
        private readonly AplicationDbContext _context;
        private readonly SayfaService _sayfaService;
        private readonly IAyarlarService _ayarlarService;
        private readonly IAltSayfaService _altSayfaService;
        private readonly IDilService _dilService;

        public SayfalarService(AplicationDbContext context, SayfaService sayfaService,
            IAyarlarService ayarlarService, IAltSayfaService altSayfaService, IDilService dilService)
        {
            _context = context;
            _sayfaService = sayfaService;
            _ayarlarService = ayarlarService;
            _altSayfaService = altSayfaService;
            _dilService = dilService;
        }

        public async Task<Sayfalar?> SoftFirstOrDefaultAsync(int id)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Sayfalar
                .AsNoTracking()
                .Include(s => s.AltSayfalari.Where(a => a.State && a.DilId == dilId))
                .FirstOrDefaultAsync(s => s.Id == id && s.State && s.DilId == dilId);
        }


        public async Task<ICollection<Sayfalar>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            // Sayfalar ve AltSayfalar ilişkisini birlikte yükleme
            var sayfalar = await _context.Sayfalar
                .AsNoTracking()
                .Where(s => s.State && s.DilId == dilId)
                .Include(s => s.AltSayfalari.Where(a => a.State && a.DilId == dilId)) // AltSayfalar'ı dahil et
                .ToListAsync();

            if (sayfalar.Count() == 0)
            {
                await _sayfaService?.SayfalariSifirla();
                sayfalar = await _context.Sayfalar
                .AsNoTracking()
                .Where(s => s.State && s.DilId == dilId)
                .Include(s => s.AltSayfalari.Where(a => a.State && a.DilId == dilId)) // AltSayfalar'ı dahil et
                .ToListAsync();
            }

            return sayfalar;
        }

        private async Task CheckIfExistingAddSayfa(int sayfaId, string baslik)
        {
            Sayfalar? sayfa = await SoftFirstOrDefaultAsync(sayfaId);
            if (sayfa == null || sayfa.SayfaBasligi == baslik)
                return;

            Sayfalar yeniKayit = new Sayfalar()
            {
                SayfaBasligi = sayfa.SayfaBasligi,
                Url = sayfa.Url,
                DilId = sayfa.DilId,
                State = false
            };
            await _context.Sayfalar.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();
            // çağırıldığında dışarıda değişiklikleri kaydedilmesi gerek.
        }

        private async Task CheckIfExistingAddAltSayfa(int altSayfaId, string baslik)
        {
            AltSayfa? altSayfa = await _altSayfaService.SoftFirstOrDefaultAsync(altSayfaId);
            if (altSayfa == null || altSayfa.AltSayfaBaslik == baslik)
                return;


            AltSayfa yeniKayit = new AltSayfa()
            {
                AltSayfaBaslik = altSayfa.AltSayfaBaslik,
                Url = altSayfa.Url,
                UstSayfaId = altSayfa.UstSayfaId,
                DilId = altSayfa.DilId,
                State = false

            };
            await _context.AltSayfa.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();
            // çağırıldığında dışarıda değişiklikleri kaydedilmesi gerek.
        }

        public async Task<bool> SoftUpdateAsync(ICollection<Sayfalar> model)
        {
            // Model'in kopyasını alarak değişikliklerden izole edelim
            foreach (var sayfa in model.ToList())
            {
                await CheckIfExistingAddSayfa(sayfa.Id, sayfa.SayfaBasligi);
                if (sayfa != null)
                {
                    var entry = _context.Entry(sayfa);
                    entry.State = EntityState.Modified; // Genel olarak güncellenecek

                    entry.Property(x => x.DilId).IsModified = false; // Güncellenmesin
                    entry.Property(x => x.AyarlarId).IsModified = false; // Güncellenmesin
                    entry.Property(x => x.Url).IsModified = false; // Güncellenmesin
                }

                if (sayfa.AltSayfalari != null)
                {
                    // Alt sayfa koleksiyonunun kopyasını alıyoruz
                    foreach (var alt in sayfa.AltSayfalari.ToList())
                    {
                        await CheckIfExistingAddAltSayfa(alt.Id, alt.AltSayfaBaslik);
                        if(alt != null)
                        {
                            var entry = _context.Entry(alt);
                            entry.State = EntityState.Modified; // Genel olarak güncellenecek

                            entry.Property(x => x.DilId).IsModified = false; // Güncellenmesin
                            entry.Property(x => x.UstSayfaId).IsModified = false; // Güncellenmesin
                            entry.Property(x => x.Url).IsModified = false; // Güncellenmesin
                        }

                    }
                }
            }

            var ayarlar = await _ayarlarService.SoftGetAyarlarAsync();
            ayarlar.DilId = await _dilService.SoftGetDilIdFromCookie();
            ayarlar!.Sayfalar = model.ToList();
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
