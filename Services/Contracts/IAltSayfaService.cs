using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IAltSayfaService
    {
        Task<SelectList?> SoftGetAllAsSelectListAsync();
        Task<AltSayfa?> SoftFirstOrDefaultAsync(int id);
    }
}
