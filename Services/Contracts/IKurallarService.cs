using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IKurallarService
    {
        Task<IEnumerable<Kurallar>> SoftGetAllAsync();
        Task<Kurallar?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(Kurallar kurallar);
        Task<bool> SoftUpdateAsync(Kurallar kurallar);
        Task<bool> SoftDeleteAsync(int id);
    }
}
