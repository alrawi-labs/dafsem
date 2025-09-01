using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IBankaBilgileriService
    {
        Task<IEnumerable<BankaBilgileri?>?> SoftGetAllAsync();
        Task<BankaBilgileri?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(BankaBilgileri bankaBilgileri);
        Task<bool> SoftUpdateAsync(BankaBilgileri bankaBilgileri);
        Task<bool> SoftDeleteAsync(int id);
    }
}
