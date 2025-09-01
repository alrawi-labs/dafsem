using dafsem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IParaBirimiService
    {
        Task<IEnumerable<ParaBirimi>> SoftGetAllAsync();
        Task<ParaBirimi?> SoftFirstOrDefaultAsync(int id);
        Task<SelectList?> SoftGeAllAsSelectListAsync();
        Task<bool> SoftAddAsync(ParaBirimi paraBirimi);
        Task<bool> SoftUpdateAsync(ParaBirimi paraBirimi);
        Task<bool> SoftDeleteAsync(int id);

    }
}
