using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IDilService
    {
        Task<ICollection<Dil>> SoftGetAllDilAsync();
        Task<Dil?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(Dil dil);
        Task<bool> SoftDeleteAsync(int id);
        List<SelectListItem> GetSupportedLanguages();
        Task<List<SelectListItem>> SoftGetAllDilAsSelectListAsync();
        Task<int> SoftGetDilIdFromCookie();
        Task<string> GetKodOfDilByID(int id);
        
    }
}
