using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class KurulKategorileriService : IKurulKategorileriService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public KurulKategorileriService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<ICollection<KurulKategorileri>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.KurulKategorileri
                .AsNoTracking()
                .Where(k => k.State && k.DilId == dilId)
                .Include(k => k.Sayfa)
                .OrderBy(u => u.Sira)
                .ToListAsync();
        }
        public async Task<List<SelectListItem>> SoftGetSiraAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var takenNumbers = await _context.KurulKategorileri
                .AsNoTracking()
                .Where(k => k.State && k.DilId == dilId)
                .Select(u => u.Sira)
                .ToListAsync();

            var availableNumbers = Enumerable.Range(1, 999)
            .Where(n => !takenNumbers.Contains(n))
            .Select(n => new SelectListItem
            {
                Value = n.ToString(),
                Text = n.ToString()
            })
            .ToList();

            return availableNumbers;
        }

        public async Task<List<SelectListItem>> SoftGetSiraAsSelectListAsync(int id)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var kurulKategorileri = await SoftFirstOrDefaultAsync(id);
            if (kurulKategorileri == null)
                return null;

            var takenNumbers = await _context.KurulKategorileri
                .AsNoTracking()
                .Where(u => u.Id != id && u.State && u.DilId == dilId) // Düzenlenen kaydın sırası dahil edilmez
                .Select(u => u.Sira)
                .ToListAsync();

            // 1-999 arasındaki boş sıra numaralarını alıyoruz ve mevcut sıra numarasını ekliyoruz
            var availableNumbers = Enumerable.Range(1, 999)
                .Where(n => !takenNumbers.Contains(n) || n == kurulKategorileri.Sira) // Mevcut sırayı dahil ediyoruz
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString(),
                    Selected = (n == kurulKategorileri.Sira) // Seçili olanı belirtiyoruz
                })
                .ToList();

            return availableNumbers;
        }

        public async Task<SelectList> SoftGetAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var kategoriler = await _context.KurulKategorileri
                       .Where(k => k.State && k.DilId == dilId)
                       .Select(k => new { k.Id, k.Baslik })
                       .ToListAsync();
            return new SelectList(kategoriler, "Id", "Baslik");
        }

        public async Task<KurulKategorileri?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.KurulKategorileri
                .AsNoTracking()
                .Include(k => k.Sayfa)
                .FirstOrDefaultAsync(k => k.Id == id && k.State);
        }

        public async Task<bool> SoftAddAsync(KurulKategorileri kurulKategorileri)
        {
            if (kurulKategorileri == null)
                return false;

            try
            {
                kurulKategorileri.DilId = await _dilService.SoftGetDilIdFromCookie();
                kurulKategorileri.State = true;
                await _context.AddAsync(kurulKategorileri);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public async Task<bool> SoftUpdateAsync(KurulKategorileri kurulKategorileri)
        {

            KurulKategorileri? model = await SoftFirstOrDefaultAsync(kurulKategorileri.Id);
            if (model == null)
                return false;

            // Yeni kaydı oluşturup ekliyoruz
            KurulKategorileri yeniKayit = new KurulKategorileri
            {
                Baslik = model.Baslik,
                SayfaId = model.SayfaId,
                KurulUyeleri = model.KurulUyeleri,
                Sira = model.Sira,
                DilId = model.DilId,
                State = false
            };

            await _context.KurulKategorileri.AddAsync(yeniKayit);
            kurulKategorileri.DilId = model.DilId;
            _context.KurulKategorileri.Update(kurulKategorileri);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<KurulKategorileri?> SoftFindAsync(int id)
        {
            var kurulKategorileri = await _context.KurulKategorileri.FindAsync(id);

            // Eğer bulunan başlığın State'i false ise null döndür
            return (kurulKategorileri != null && kurulKategorileri.State) ? kurulKategorileri : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var kurulKategorileri = await SoftFindAsync(id);
            if (kurulKategorileri == null)
                return false;

            kurulKategorileri.State = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
