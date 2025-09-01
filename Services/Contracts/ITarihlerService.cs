using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface ITarihlerService
    {
        Task<IEnumerable<Tarihler?>?> SoftGetAllAsync();
        Task<Tarihler?> SoftFirstOrDefaultAsync(int id);
        Task<Tarihler?> SoftFirstOrDefaultAsync();
        Task<bool> SoftAddAsync(Tarihler? tarihler);
        Task<bool> SoftUpdateAsync(Tarihler entity);
        bool SoftTarihlerExists(int id);
        Task<bool> SoftDeleteAsync(int id);
    }
}
