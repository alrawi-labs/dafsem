using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IKonaklamaService
    {
        Task<IEnumerable<Konaklama>> SoftGetAllAsync();
        Task<SelectList?> SoftGeAllAsSelectListAsync();
        Task<Konaklama?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(Konaklama konaklama);
        Task<bool> SoftUpdateAsync(Konaklama konaklama);
        Task<bool> SoftDeleteAsync(int id);
        
    }
}
