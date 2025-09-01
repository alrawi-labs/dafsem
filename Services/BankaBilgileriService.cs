using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dafsem.Services
{
    public class BankaBilgileriService : IBankaBilgileriService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public BankaBilgileriService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<IEnumerable<BankaBilgileri?>?> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();
            IEnumerable<BankaBilgileri> model = await _context.BankaBilgileri
                .AsNoTracking()
                .Where(t => t.State && t.DilId == dilId)
                .ToListAsync();
            return model;
        }
        private async Task<BankaBilgileri?> SoftFindAsync(int id)
        {
            var bankaBilgileri = await _context.BankaBilgileri.FindAsync(id);

            // Eğer bulunan başlığın State'i false ise null döndür
            return (bankaBilgileri != null && bankaBilgileri.State) ? bankaBilgileri : null;
        }
        public async Task<BankaBilgileri?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.BankaBilgileri.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id && t.State);
        }

        public async Task<bool> SoftAddAsync(BankaBilgileri bankaBilgileri)
        {
            if (bankaBilgileri == null)
                return false;

            try
            {
                bankaBilgileri.DilId = await _dilService.SoftGetDilIdFromCookie();
                bankaBilgileri.State = true;
                bankaBilgileri.IBAN = bankaBilgileri.FormatIBAN(bankaBilgileri.IBAN);
                await _context.BankaBilgileri.AddAsync(bankaBilgileri);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ValidationException ex)
            {
                throw new ValidationException(ex.Message);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SoftUpdateAsync(BankaBilgileri bankaBilgileri)
        {

            var model = await SoftFirstOrDefaultAsync(bankaBilgileri.Id);
            if (model == null)
                return false;

            var yeniKayit = new BankaBilgileri
            {
                BankaAdi = model.BankaAdi,
                HesapSahibiAdi = model.HesapSahibiAdi,
                IBAN = model.IBAN,
                DilId = model.DilId,
                State = false
            };

            await _context.BankaBilgileri.AddAsync(yeniKayit);

            bankaBilgileri.IBAN = bankaBilgileri.FormatIBAN(bankaBilgileri.IBAN);
            bankaBilgileri.DilId = model.DilId;
            _context.BankaBilgileri.Update(bankaBilgileri);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var bankaBilgileri = await SoftFindAsync(id);
            if (bankaBilgileri == null)
                return false;

            bankaBilgileri.State = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
