using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services.Contracts
{
    public interface IHizmetTuruService
    {
        Task<IEnumerable<HizmetTuru>> SoftGetAllAsync();
        Task<SelectList?> SoftGeAllAsSelectListAsync();
        Task<HizmetTuru?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(HizmetTuru hizmetTuru);
        Task<bool> SoftUpdateAsync(HizmetTuru hizmetTuru);
        Task<bool> SoftDeleteAsync(int id);
        
    }
}
