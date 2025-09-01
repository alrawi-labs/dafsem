using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class UcretlerService : IUcretlerService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public UcretlerService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }
        public async Task<IEnumerable<Ucretler?>?> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Ucretler
                .AsNoTracking()
                .Where(u => u.State && u.DilId == dilId)
                .Include(u => u.Birim)
                .ToListAsync();
        }
        public async Task<Ucretler?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Ucretler.AsNoTracking().Include(u => u.Birim).FirstOrDefaultAsync(u => u.State && u.Id == id);
        }
        public async Task<bool> SoftAddAsync(Ucretler ucretler)
        {
            if (ucretler == null)
                return false;

            try
            {
                ucretler.DilId = await _dilService.SoftGetDilIdFromCookie();
                ucretler.State = true;
                _context.Entry(ucretler.Birim).State = EntityState.Unchanged;
                await _context.Ucretler.AddAsync(ucretler);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public async Task<bool> SoftUpdateAsync(Ucretler ucretler)
        {
            Ucretler? model = await SoftFirstOrDefaultAsync(ucretler.Id);
            if (model == null)
                return false;


            Ucretler yeniKayit = new Ucretler
            {
                Baslik = model.Baslik,
                Birim = model.Birim,
                Ucret = model.Ucret,
                DilId = model.DilId,
                State = false
            };
            _context.Entry(yeniKayit.Birim).State = EntityState.Unchanged;

            await _context.Ucretler.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();

            // Daha önce takip edilen nesneyi takipten çıkar
            _context.Entry(model.Birim).State = EntityState.Detached;
            ucretler.DilId  = model.DilId;
            _context.Update(ucretler);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<Ucretler?> SoftFindAsync(int id)
        {
            Ucretler? ucretler = await _context.Ucretler.FindAsync(id);
            return (ucretler != null && ucretler.State) ? ucretler : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            Ucretler? ucretler = await SoftFindAsync(id);
            if (ucretler == null)
                return false;

            ucretler.State = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
