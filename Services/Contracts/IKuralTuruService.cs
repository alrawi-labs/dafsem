using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IKuralTuruService
    {
        Task<IEnumerable<KuralTuru>> SoftGetAllAsync();
        Task<KuralTuru?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(KuralTuru kuralTuru);
        Task<bool> SoftUpdateAsync(KuralTuru kuralTuru);
        Task<bool> SoftDeleteAsync(int id);
        Task<SelectList?> SoftGeAllAsSelectListAsync();


    }
}
