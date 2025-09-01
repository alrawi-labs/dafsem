using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class TarihlerService : ITarihlerService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public TarihlerService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<Tarihler?>?> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            // her dil için tarihin yazılış halini gösterirsem daha güzel olur
            return await _context.Tarihler.AsNoTracking().Where(t => t.State && t.DilId == dilId).ToListAsync();
        }

        private async Task<Tarihler?> SoftFindAsync(int id)
        {
            var tarih = await _context.Tarihler.FindAsync(id);
            return (tarih != null && tarih.State) ? tarih : null;
        }

        public async Task<bool> SoftAddAsync(Tarihler? tarihler)
        {
            if (tarihler == null)
                return false;

            try
            {
                tarihler.DilId = await _dilService.SoftGetDilIdFromCookie();
                tarihler.State = true;
                await _context.Tarihler.AddAsync(tarihler);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<Tarihler?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Tarihler.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id && t.State);
        }

        public async Task<Tarihler?> SoftFirstOrDefaultAsync()
        {
            return await _context.Tarihler.AsNoTracking().FirstOrDefaultAsync(t => t.State);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var tarih = await SoftFindAsync(id);
            if (tarih == null)
                return false;

            tarih.State = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftUpdateAsync(Tarihler entity)
        {
            var model = await SoftFirstOrDefaultAsync(entity.Id);
            if (model == null)
                return false;

            var yeniKayit = new Tarihler
            {
                Tarih = model.Tarih,
                Aciklama = model.Aciklama,
                DilId = model.DilId,
                State = false
            };

            await _context.Tarihler.AddAsync(yeniKayit);
            entity.DilId = model.DilId;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool SoftTarihlerExists(int id)
        {
            return _context.Tarihler.AsNoTracking().Any(e => e.Id == id && e.State);
        }
    }
}
