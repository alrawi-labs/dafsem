using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class UnvanlarService : IUnvanlarService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public UnvanlarService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }
        public async Task<bool> SoftAddAsync(Unvan unvan)
        {
            if (unvan == null)
                return false;

            try
            {
                unvan.DilId = await _dilService.SoftGetDilIdFromCookie();
                unvan.State = true;
                await _context.AddAsync(unvan);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var unvan = await SoftFindAsync(id);
            if (unvan == null)
                return false;

            unvan.State = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<Unvan?> SoftFindAsync(int id)
        {
            if (id == 0)
                return null; // Geçersiz parametre kontrolü

            var unvan = await _context.Unvan.FindAsync(id);

            // Eğer bulunan kaydın State'i false ise null döndür
            return (unvan != null && unvan.State) ? unvan : null;
        }

        public async Task<Unvan?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Unvan.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && u.State);
        }

        public async Task<IEnumerable<Unvan?>?> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            IEnumerable<Unvan> model = await _context.Unvan
                .AsNoTracking()
                .Where(u => u.State && u.DilId == dilId)
                .OrderBy(u => u.Sira)
                .ToListAsync();
            return model;
        }

        public async Task<SelectList> SoftGetAsSelectItemAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var unvanlar = await _context.Unvan
                .Where(u => u.State && u.DilId == dilId)
               .Select(k => new { k.Id, k.UnvanAdi })
               .ToListAsync();
            return new SelectList(unvanlar, "Id", "UnvanAdi");
        }

        public async Task<bool> SoftUpdateAsync(Unvan entity)
        {
            var model = await SoftFirstOrDefaultAsync(entity.Id);
            if (model == null)
                return false;

            if (await SoftIsSiraUniqueAsync(entity.Sira, entity.Id) == false)
            {
                throw new InvalidOperationException("Seçmiş olduğunuz sıra değeri hatalıdır. Lütfen tekrar deneyin.");
            }

            // Yeni kaydı oluşturup ekliyoruz
            var yeniKayit = new Unvan
            {
                Sira = model.Sira,
                UnvanAdi = model.UnvanAdi,
                DilId = model.Id,
                State = false
            };

            await _context.Unvan.AddAsync(yeniKayit);
            entity.DilId = await _dilService.SoftGetDilIdFromCookie();
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // onunla dahil sıra bilgileri getiriyor
        public async Task<List<int>> SoftGetSira()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Unvan
                .Where(s => s.State && s.DilId == dilId)
                .Select(u => u.Sira)
                .ToListAsync();
        }
        // Kendisi hariç sıra bilgileir getiriyro
        public async Task<List<int>> SoftGetSiraWithOut(int id)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Unvan
                .Where(s => s.State && s.Id != id && s.DilId == dilId)
                .Select(u => u.Sira)
                .ToListAsync();
        }
        private async Task<bool> SoftIsSiraUniqueAsync(int sira, int? id = null)
        {
            return !await _context.Unvan
                .AnyAsync(u => u.Sira == sira && u.State == true && (id == null || u.Id != id));
        }
    }
}
