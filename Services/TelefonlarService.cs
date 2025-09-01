using Azure.Core;
using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class TelefonlarService : ITelefonlarService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;

        public TelefonlarService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<bool> CheckValidTelefonAsync(TelefonDto telefon)
        {
            if (string.IsNullOrEmpty(telefon.Tel))
            {
                throw new ArgumentException("Telefon numarası gereklidir.");
            }

            if (telefon.Dahili == null || telefon.Dahili.Length == 0)
            {
                throw new ArgumentException("Dahili numara gereklidir.");
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> SoftAddAsync(Telefonlar telefon)
        {
            if (telefon == null)
                return false;

            try
            {
                telefon.DilId = await _dilService.SoftGetDilIdFromCookie();
                telefon.State = true;

                await _context.Telefonlar.AddAsync(telefon);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        private async Task<Telefonlar?> SoftFindAsync(int id)
        {
            var telefon = await _context.Telefonlar.FindAsync(id);
            return (telefon != null && telefon.State) ? telefon : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var telefon = await SoftFindAsync(id);
            if (telefon == null)
            {
                throw new KeyNotFoundException("Telefon numarası bulunamadı.");
            }

            telefon.State = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Telefonlar?> SoftFirstOrDefault(int id)
        {
            return await _context.Telefonlar.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id && h.State);
        }

        public async Task<bool> SoftUpdateAsync(int id, TelefonDto telefon)
        {
            Telefonlar? varolan = await SoftFirstOrDefault(id);
            if (varolan == null)
                throw new FileNotFoundException("Telefon bulunamadı");
            

            bool valid = await CheckValidTelefonAsync(telefon);
            if (!valid)
                return false;

            

            Telefonlar yeniKayit = new Telefonlar
            {
                Tel = varolan.Tel,
                Dahili = string.Join("/", varolan.Dahili),  // Dahili numaralarını "/" ile birleştirerek kaydediyoruz
                IletisimId = varolan!.IletisimId,
                DilId = varolan!.DilId,
                State = false
            };

            await _context.Telefonlar.AddAsync(yeniKayit);
            await _context.SaveChangesAsync();

            varolan.Tel = telefon.Tel;
            varolan.Dahili = string.Join("/", telefon.Dahili);

            _context.Telefonlar.Update(varolan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
