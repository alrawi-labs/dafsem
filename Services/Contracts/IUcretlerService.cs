using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IUcretlerService
    {
        Task<IEnumerable<Ucretler?>?> SoftGetAllAsync();
        Task<Ucretler?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(Ucretler ucretler);
        Task<bool> SoftUpdateAsync(Ucretler ucretler);
        Task<bool> SoftDeleteAsync(int id);
       
    }
}
