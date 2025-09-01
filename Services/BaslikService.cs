using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class BaslikService : IBaslikService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public BaslikService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }
        public async Task<IEnumerable<Basliklar?>?> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            IEnumerable<Basliklar> model = await _context.Basliklar
                .AsNoTracking()
                .Where(b => b.State && b.DilId == dilId)
                .ToListAsync();
            return model;
        }
        private async Task<Basliklar?> SoftFindAsync(int id)
        {
            var baslik = await _context.Basliklar.FindAsync(id);

            // Eğer bulunan başlığın State'i false ise null döndür
            return (baslik != null && baslik.State) ? baslik : null;
        }
        public async Task<bool> SoftAddAsync(Basliklar? basliklar)
        {
            if (basliklar == null)
                return false;

            try
            {
                basliklar.DilId = await _dilService.SoftGetDilIdFromCookie();
                basliklar.State = true;
                await _context.AddAsync(basliklar);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }


            return true;
        }
        public async Task<Basliklar?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Basliklar
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id && b.State);
        }

        public async Task<Basliklar?> SoftFirstOrDefaultAsync()
        {
            return await _context.Basliklar
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.State);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var baslik = await SoftFindAsync(id);
            if (baslik == null)
                return false;

            baslik.State = false;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SoftUpdateAsync(Basliklar entity)
        {
            var model = await SoftFirstOrDefaultAsync(entity.Id);
            if (model == null)
                return false;

            // Yeni kaydı oluşturup ekliyoruz
            var yeniKayit = new Basliklar
            {
                Baslik = model.Baslik,
                DilId = model.DilId,
                State = false
            };

            await _context.Basliklar.AddAsync(yeniKayit);
            entity.DilId = model.DilId;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool SoftBasliklarExists(int id)
        {
            return _context.Basliklar.AsNoTracking().Any(e => e.Id == id && e.State);
        }



    }
}
