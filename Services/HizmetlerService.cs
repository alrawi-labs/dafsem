using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class HizmetlerService : IHizmetlerService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public HizmetlerService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<Hizmetler>> SoftGetAllHizmetlerAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Hizmetler
                .AsNoTracking()
                .Where(h => h.State && h.DilId == dilId)
                .Include(a => a.Turu)
                .ToListAsync();
        }

        public async Task<Hizmetler?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Hizmetler
                .AsNoTracking()
                .Include(a => a.Turu)
                .FirstOrDefaultAsync(h => h.Id == id && h.State);
        }

        public async Task<bool> SoftAddAsync(Hizmetler hizmetler)
        {
            if (hizmetler == null)
                return false;

            try
            {
                hizmetler.DilId = await _dilService.SoftGetDilIdFromCookie();
                hizmetler.State = true;
                await _context.Hizmetler.AddAsync(hizmetler);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public async Task<bool> SoftUpdateAsync(Hizmetler hizmetler)
        {
            Hizmetler? model = await SoftFirstOrDefaultAsync(hizmetler.Id);
            if (model == null)
                return false;

            Hizmetler yeniKayit = new Hizmetler
            {
                Hizmet = model.Hizmet,
                HizmetTuruId = model.HizmetTuruId,
                DilId = model.DilId,
                State = false
            };

            await _context.Hizmetler.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();
            hizmetler.DilId = model.DilId;
            _context.Hizmetler.Update(hizmetler);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<Hizmetler?> SoftFindAsync(int id)
        {
            var hizmetler = await _context.Hizmetler.FindAsync(id);
            return (hizmetler != null && hizmetler.State) ? hizmetler : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            Hizmetler? hizmetler = await SoftFindAsync(id);
            if (hizmetler == null)
                return false;

            hizmetler.State = false;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
