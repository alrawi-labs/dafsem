using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class KonaklamaService : IKonaklamaService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public KonaklamaService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<Konaklama>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Konaklama
                .AsNoTracking()
                .Where(k => k.State && k.DilId == dilId)
                .ToListAsync();
        }
        public async Task<SelectList?> SoftGeAllAsSelectListAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var model = await _context.Konaklama
                .AsNoTracking()
                .Where(k => k.State && k.DilId == dilId)
                .Select(k => new { k.Id, k.KonaklamaEvi })
                .ToListAsync();
            return new SelectList(model, "Id", "KonaklamaEvi");
        }

        public async Task<Konaklama?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Konaklama.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id && k.State);
        }

        public async Task<bool> SoftAddAsync(Konaklama konaklama)
        {
            if (konaklama == null)
                return false;

            try
            {
                konaklama.DilId = await _dilService.SoftGetDilIdFromCookie();
                konaklama.State = true;
                await _context.Konaklama.AddAsync(konaklama);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }
        public async Task<bool> SoftUpdateAsync(Konaklama konaklama)
        {
            Konaklama? model = await SoftFirstOrDefaultAsync(konaklama.Id);
            if (model == null)
                return false;


            Konaklama yeniKayit = new Konaklama
            {
                Adres = model.Adres,
                KonaklamaEvi = model.KonaklamaEvi,
                Eposta = model.Eposta,
                KahvaltiDahilMi = model.KahvaltiDahilMi,
                Odalar = model.Odalar,
                WebSitesi = model.WebSitesi,
                Tel = model.Tel,
                YildizSayisi = model.YildizSayisi,
                DilId = model.DilId,
                State = false
            };

            await _context.Konaklama.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();
            konaklama.DilId = model.DilId;
            _context.Konaklama.Update(konaklama);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<Konaklama?> FirstOrDefaultAsync(int id)
        {
            var konaklama = await _context.Konaklama.Include(k => k.Odalar.Where(o => o.State)).FirstOrDefaultAsync(k => k.Id == id && k.State);
            return konaklama;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            Konaklama? konaklama = await FirstOrDefaultAsync(id);
            if (konaklama == null)
                return false;

            konaklama.State = false;
            foreach (var item in konaklama.Odalar)
            {
                item.State = false;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
