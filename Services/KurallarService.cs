using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class KurallarService : IKurallarService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public KurallarService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }
        public async Task<IEnumerable<Kurallar>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Kurallar
                .AsNoTracking()
                .Where(k => k.State && k.DilId == dilId)
                .Include(a => a.Turu)
                .ToListAsync();
        }
        public async Task<Kurallar?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Kurallar.AsNoTracking().Include(a => a.Turu)
                           .FirstOrDefaultAsync(m => m.Id == id && m.State);
        }
        public async Task<bool> SoftAddAsync(Kurallar kurallar)
        {
            if (kurallar == null)
                return false;

            try
            {
                kurallar.DilId = await _dilService.SoftGetDilIdFromCookie();
                kurallar.State = true;
                await _context.Kurallar.AddAsync(kurallar);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public async Task<bool> SoftUpdateAsync(Kurallar kurallar)
        {
            Kurallar? model = await SoftFirstOrDefaultAsync(kurallar.Id);
            if (model == null)
                return false;

            Kurallar yeniKayit = new Kurallar
            {
                Metin = model.Metin,
                KuralTuruId = model.KuralTuruId,
                DilId = model.DilId,
                State = false
            };

            await _context.Kurallar.AddAsync(yeniKayit);

            kurallar.DilId = model.DilId;
            _context.Kurallar.Update(kurallar);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<Kurallar?> SoftFindAsync(int id)
        {
            var kurallar = await _context.Kurallar.FindAsync(id);
            return (kurallar != null && kurallar.State) ? kurallar : null;
        }
        public async Task<bool> SoftDeleteAsync(int id)
        {
            Kurallar? kurallar = await SoftFindAsync(id);
            if (kurallar == null)
                return false;

            kurallar.State = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
