using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IBaslikService
    {
        Task<IEnumerable<Basliklar?>?> SoftGetAllAsync();
        Task<bool> SoftAddAsync(Basliklar? basliklar);
        Task<Basliklar?> SoftFirstOrDefaultAsync(int id);
        Task<Basliklar?> SoftFirstOrDefaultAsync();
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> SoftUpdateAsync(Basliklar entity);
        bool SoftBasliklarExists(int id);
    }
}
