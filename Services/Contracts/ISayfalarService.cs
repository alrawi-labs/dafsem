using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface ISayfalarService
    {
        Task<ICollection<Sayfalar>> SoftGetAllAsync();
        Task<Sayfalar?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftUpdateAsync(ICollection<Sayfalar> model);

    }
}
