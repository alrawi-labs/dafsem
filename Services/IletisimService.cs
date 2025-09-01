using Azure.Core;
using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class IletisimService : IIletisimService
    {
        private readonly AplicationDbContext _context;
        private readonly ITelefonlarService _telefonlarService;
        private readonly IDilService _dilService;

        public IletisimService(AplicationDbContext context, ITelefonlarService telefonlarService, IDilService dilService)
        {
            _context = context;
            _telefonlarService = telefonlarService;
            _dilService = dilService;
        }
        public async Task<Iletisim?> SoftGetLastAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            var iletisim = await _context.Iletisim.AsNoTracking()
                         .Where(i => i.State && i.DilId == dilId)
                         .Include(i => i.Telefonlar.Where(t => t.State && t.DilId == dilId))
                         .OrderByDescending(i => i.Id)
                         .FirstOrDefaultAsync();

            if (iletisim != null)
                return iletisim;

            if (!await _context.Iletisim.Where(i => i.State && i.DilId == dilId).AnyAsync())
            {
                iletisim = new Iletisim();
                iletisim.DilId = dilId;
                await _context.Iletisim.AddAsync(iletisim);
                await _context.SaveChangesAsync();
                return iletisim;
            }

            return await _context.Iletisim.AsNoTracking()
                .Where(t => t.DilId == dilId)
                    .Include(i => i.Telefonlar.Where(t => t.State && t.DilId == dilId))
                    .OrderByDescending(b => b.Id)
                    .FirstOrDefaultAsync();
        }

        public async Task<bool> SoftTelefonAddAsync(TelefonDto telefon)
        {
            try
            {
                bool valid = await _telefonlarService.CheckValidTelefonAsync(telefon);
                if (!valid)
                    return false;

                Iletisim? varolan = await SoftGetLastAsync();

                Telefonlar yeniKayit = new Telefonlar
                {
                    Tel = telefon.Tel,
                    Dahili = string.Join("/", telefon.Dahili),  // Dahili numaralarını "/" ile birleştirerek kaydediyoruz
                    IletisimId = varolan!.Id,
                    DilId = varolan!.Id,
                    State = true
                };


                bool result = await _telefonlarService.SoftAddAsync(yeniKayit);
                if (!result)
                    return false;

                varolan.Telefonlar?.Add(yeniKayit);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }

            return true;
        }
        private async Task<int> GetValidIletisimID()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.Iletisim.AsNoTracking()
                         .Where(i => i.State && i.DilId == dilId)
                         .OrderByDescending(i => i.Id)
                         .Select(i => i.Id)
                         .FirstOrDefaultAsync();
        }
        public async Task<ICollection<Telefonlar>> SoftGetTelefonlar()
        {
            int id = await GetValidIletisimID();
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            return await _context.Telefonlar
                .AsNoTracking()
                .Where(t => t.IletisimId == id && t.State && t.DilId == dilId)
                .ToListAsync();
        }
        public async Task<bool> SoftUpdateAsync(Iletisim iletisim)
        {
            if (iletisim == null)
                return false;

            Iletisim? model = await SoftGetLastAsync();

            if (model == null)
            {
                try
                {
                    // daha önce hiçbir kayıt olmadığı için yeni gelen kaydı eklemek
                    iletisim.DilId = await _dilService.SoftGetDilIdFromCookie();
                    await _context.Iletisim.AddAsync(iletisim);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
            }

            Iletisim yeniKayit = new Iletisim() // telefonlar hep 1. kayda tabi olduğunu varsaydığımız için tutmaya gerek yoktur, sistemde iletişim kaydı silme seçeneği yoktur
            {
                Adres = model.Adres,
                Eposta = model.Eposta,
                DilId = model.DilId,
                State = false
            };

            await _context.Iletisim.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();
            iletisim.DilId = model.DilId;
            _context.Update(iletisim);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
