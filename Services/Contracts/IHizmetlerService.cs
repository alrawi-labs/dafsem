using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IHizmetlerService
    {
        Task<IEnumerable<Hizmetler>> SoftGetAllHizmetlerAsync();
        Task<Hizmetler?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(Hizmetler hizmetler);
        Task<bool> SoftUpdateAsync(Hizmetler hizmetler);
        Task<bool> SoftDeleteAsync(int id);
    }
}
