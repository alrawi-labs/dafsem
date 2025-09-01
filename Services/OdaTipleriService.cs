using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class OdaTipleriService : IOdaTipleriService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public OdaTipleriService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }
        public async Task<IEnumerable<OdaTipleri>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.OdaTipleri
                .AsNoTracking()
                .Where(o => o.State && o.DilId == dilId)
                .Include(o => o.Birim)
                .Include(o => o.KonaklamaEvi)
                .ToListAsync();
        }

        public async Task<OdaTipleri?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.OdaTipleri
                .AsNoTracking()
                .Include(o => o.Birim)
                .Include(o => o.KonaklamaEvi)
                .FirstOrDefaultAsync(o => o.Id == id && o.State);
        }

        public async Task<bool> SoftAddAsync(OdaTipleri odaTipleri)
        {
            if (odaTipleri == null)
                return false;

            try
            {
                odaTipleri.DilId = await _dilService.SoftGetDilIdFromCookie();
                odaTipleri.State = true;
                // Bunlar geçici olarak kalacaktır
                _context.Entry(odaTipleri.KonaklamaEvi).State = EntityState.Unchanged;
                _context.Entry(odaTipleri.Birim).State = EntityState.Unchanged;

                await _context.OdaTipleri.AddAsync(odaTipleri);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public async Task<List<OdaTipleriDto>> SoftGetRoomsByKonaklamaIdAsync(int konaklamaId)
        {
            var odalar = await _context.OdaTipleri
                .Where(o => o.KonaklamaEvi.Id == konaklamaId)
                .Select(o => new OdaTipleriDto
                {
                    Id = o.Id,
                    OdaTipi = o.OdaTipi,
                    Ucret = o.Ucret,
                    BirimSembol = o.Birim != null ? o.Birim.Sembol : ""
                })
                .ToListAsync();

            return odalar;
        }

        public async Task<bool> SoftUpdateAsync(OdaTipleri odaTipleri)
        {
            OdaTipleri? model = await SoftFirstOrDefaultAsync(odaTipleri.Id);
            if (model == null)
                return false;


            OdaTipleri yeniKayit = new OdaTipleri
            {
                KonaklamaEvi = model.KonaklamaEvi,
                Ucret = model.Ucret,
                OdaTipi = model.OdaTipi,
                Birim = model.Birim,
                DilId = model.DilId,
                State = false
            };

            // Bunlar geçici olarak kalacaktır
            _context.Entry(yeniKayit.KonaklamaEvi).State = EntityState.Unchanged;
            _context.Entry(yeniKayit.Birim).State = EntityState.Unchanged;


            await _context.OdaTipleri.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();

            // Bunlar geçici olarak kalacaktır
            _context.Entry(model.KonaklamaEvi).State = EntityState.Detached;
            _context.Entry(model.Birim).State = EntityState.Detached;
            odaTipleri.DilId = model.DilId;
            _context.OdaTipleri.Update(odaTipleri);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<OdaTipleri?> SoftFindAsync(int id)
        {
            var odaTipleri = await _context.OdaTipleri.FindAsync(id);
            return (odaTipleri != null && odaTipleri.State) ? odaTipleri : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            OdaTipleri? odaTipleri = await SoftFindAsync(id);
            if (odaTipleri == null)
                return false;

            odaTipleri.State = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
