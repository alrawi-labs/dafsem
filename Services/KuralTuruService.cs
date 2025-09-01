using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class KuralTuruService : IKuralTuruService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public KuralTuruService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<KuralTuru>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.KuralTuru
                .AsNoTracking()
                .Where(k => k.State && k.DilId == dilId)
                .Include(s => s.Sayfa)
                .ToListAsync();
        }

        public async Task<SelectList?> SoftGeAllAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var model = await _context.KuralTuru
                .AsNoTracking().Where(p => p.State && p.DilId == dilId)
                .Select(k => new { k.Id, k.Tur })
                .ToListAsync();
            return new SelectList(model, "Id", "Tur");
        }

        public async Task<KuralTuru?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.KuralTuru.AsNoTracking().Where(k => k.State).Include(s => s.Sayfa).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> SoftAddAsync(KuralTuru kuralTuru)
        {
            if (kuralTuru == null)
                return false;

            try
            {
                kuralTuru.DilId = await _dilService.SoftGetDilIdFromCookie();
                kuralTuru.State = true;
                await _context.KuralTuru.AddAsync(kuralTuru);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        public async Task<bool> SoftUpdateAsync(KuralTuru kuralTuru)
        {
            KuralTuru? model = await SoftFirstOrDefaultAsync(kuralTuru.Id);
            if (model == null)
                return false;

            KuralTuru yeniKayit = new KuralTuru
            {
                Tur = model.Tur,
                SayfaId = model.SayfaId,
                DilId = model.DilId,
                State = false
            };

            await _context.KuralTuru.AddAsync(yeniKayit);
            kuralTuru.DilId = model.DilId;
            _context.KuralTuru.Update(kuralTuru);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<KuralTuru?> SoftFindAsync(int id)
        {
            KuralTuru? kuralTuru = await _context.KuralTuru.FindAsync(id);
            return (kuralTuru != null && kuralTuru.State) ? kuralTuru : null;
        }

        private async Task<IEnumerable<Kurallar?>> SoftFindKurallarByKuralTuru(int kuralTuruId)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Kurallar
                .Include(h => h.Turu)
                .Where(h => h.Turu.Id == kuralTuruId && h.State && h.DilId == dilId)
                .ToListAsync();
        }
        private async Task<bool> SoftDeleteKurallarTuruAsync(int kuralTuruId)
        {
            // Bağlı kurallar getir
            var kurallar = await SoftFindKurallarByKuralTuru(kuralTuruId);

            // Bağlı hizmetleri de soft delete yap
            foreach (var kural in kurallar)
            {
                kural.State = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SoftDeleteAsync(int id)
        {
            KuralTuru? kuralTuru = await SoftFindAsync(id);
            if (kuralTuru == null)
                return false;

            // İlgili hizmet türünü silme işlemi
            try
            {
                kuralTuru.State = false;
            }
            catch (Exception)
            {
                return false;
            }

            // Bu kural türüne bağlı olan kuraları da silme
            bool result = await SoftDeleteKurallarTuruAsync(kuralTuru.Id);
            if (!result)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
