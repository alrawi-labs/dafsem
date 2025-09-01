using dafsem.Models;
using dafsem.Context;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services
{
    public class ParaBirimiService : IParaBirimiService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public ParaBirimiService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<ParaBirimi>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.ParaBirimi
                .AsNoTracking()
                .Where(p => p.State && p.DilId == dilId)
                .ToListAsync();
        }
        public async Task<SelectList?> SoftGeAllAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var model = await _context.ParaBirimi
                .AsNoTracking()
                .Where(p => p.State && p.DilId == dilId)
                .Select(k => new { k.Id, k.Sembol })
                .ToListAsync();
            return new SelectList(model, "Id", "Sembol");
        }

        public async Task<ParaBirimi?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.ParaBirimi.AsNoTracking().FirstOrDefaultAsync(p => p.State && p.Id == id);
        }

        public async Task<bool> SoftAddAsync(ParaBirimi paraBirimi)
        {
            if (paraBirimi == null)
                return false;

            try
            {
                paraBirimi.DilId = await _dilService.SoftGetDilIdFromCookie();
                paraBirimi.State = true;
                await _context.AddAsync(paraBirimi);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public async Task<bool> SoftUpdateAsync(ParaBirimi paraBirimi)
        {
            ParaBirimi? model = await SoftFirstOrDefaultAsync(paraBirimi.Id);
            if (model == null)
                return false;


            ParaBirimi yeniKayit = new ParaBirimi
            {
                Ad = model.Ad,
                Kod = model.Kod,
                Sembol = model.Sembol,
                DilId = model.DilId,
                State = false
            };

            await _context.ParaBirimi.AddAsync(yeniKayit);
            paraBirimi.DilId = model.DilId;
            _context.Update(paraBirimi);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<ParaBirimi?> SoftFindAsync(int id)
        {
            ParaBirimi? paraBirimi = await _context.ParaBirimi.FindAsync(id);
            return (paraBirimi != null && paraBirimi.State) ? paraBirimi : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            ParaBirimi? paraBirimi = await SoftFindAsync(id);
            if (paraBirimi == null)
                return false;

            paraBirimi.State = false;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
