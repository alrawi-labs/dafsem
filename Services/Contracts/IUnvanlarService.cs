using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IUnvanlarService
    {
        Task<IEnumerable<Unvan?>?> SoftGetAllAsync();
        Task<SelectList> SoftGetAsSelectItemAsync();
        Task<Unvan?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(Unvan unvan);
        Task<bool> SoftUpdateAsync(Unvan unvan);
        Task<bool> SoftDeleteAsync(int id);
        Task<List<int>> SoftGetSira();
        Task<List<int>> SoftGetSiraWithOut(int id);
    }
}
