using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class HizmetTuruService : IHizmetTuruService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public HizmetTuruService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<HizmetTuru>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.HizmetTuru
                .AsNoTracking()
                .Where(h => h.State && h.DilId == dilId)
                .ToListAsync();
        }

        public async Task<SelectList?> SoftGeAllAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var model = await _context.HizmetTuru
                .AsNoTracking()
                .Where(p => p.State && p.DilId == dilId)
                .Select(k => new { k.Id, k.Tur })
                .ToListAsync();
            return new SelectList(model, "Id", "Tur");
        }
        public async Task<HizmetTuru?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.HizmetTuru.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id && k.State);
        }

        public async Task<bool> SoftAddAsync(HizmetTuru hizmetTuru)
        {
            if (hizmetTuru == null)
                return false;

            try
            {
                hizmetTuru.DilId = await _dilService.SoftGetDilIdFromCookie();
                hizmetTuru.State = true;
                await _context.HizmetTuru.AddAsync(hizmetTuru);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        public async Task<bool> SoftUpdateAsync(HizmetTuru hizmetTuru)
        {
            HizmetTuru? model = await SoftFirstOrDefaultAsync(hizmetTuru.Id);
            if (model == null)
                return false;

            HizmetTuru yeniKayit = new HizmetTuru
            {
                Tur = model.Tur,
                DilId = model.DilId,
                State = false
            };

            await _context.HizmetTuru.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();
            hizmetTuru.DilId = model.DilId;
            _context.HizmetTuru.Update(hizmetTuru);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<HizmetTuru?> SoftFindAsync(int id)
        {
            HizmetTuru? hizmetTuru = await _context.HizmetTuru.FindAsync(id);
            return (hizmetTuru != null && hizmetTuru.State) ? hizmetTuru : null;
        }
        private async Task<IEnumerable<Hizmetler?>> SoftFindHizmetByHizmetTuru(int HizmetTuruId)
        {
            return await _context.Hizmetler.Include(h => h.Turu).Where(h => h.Turu.Id == HizmetTuruId && h.State).ToListAsync();
        }
        private async Task<bool> SoftDeleteHizmetTuruAsync(int hizmetTuruId)
        {

            // Bağlı hizmetleri getir
            var hizmetler = await SoftFindHizmetByHizmetTuru(hizmetTuruId);

            // Bağlı hizmetleri de soft delete yap
            foreach (var hizmet in hizmetler)
            {
                hizmet.State = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SoftDeleteAsync(int id)
        {
            HizmetTuru? hizmetTuru = await SoftFindAsync(id);
            if (hizmetTuru == null)
                return false;

            // İlgili hizmet türünü silme işlemi
            try
            {
                hizmetTuru.State = false;
            }
            catch (Exception)
            {
                return false;
            }

            // Bu hizmet türüne bağlı olan hizmetleri de silme
            bool result = await SoftDeleteHizmetTuruAsync(hizmetTuru.Id);
            if (!result)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
