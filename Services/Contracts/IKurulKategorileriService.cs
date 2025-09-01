using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IKurulKategorileriService
    {
        Task<ICollection<KurulKategorileri>> SoftGetAllAsync();
        Task<List<SelectListItem>> SoftGetSiraAsSelectListAsync(int id);
        Task<List<SelectListItem>> SoftGetSiraAsSelectListAsync();
        Task<SelectList> SoftGetAsSelectListAsync();
        Task<KurulKategorileri?> SoftFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(KurulKategorileri kurulKategorileri);
        Task<bool> SoftUpdateAsync(KurulKategorileri kurulKategorileri);
        Task<bool> SoftDeleteAsync(int id);


    }
}
