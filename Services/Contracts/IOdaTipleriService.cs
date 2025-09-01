using dafsem.Models;
using dafsem.Models.ViewModels;

namespace dafsem.Services.Contracts
{
    public interface IOdaTipleriService
    {
        Task<IEnumerable<OdaTipleri>> SoftGetAllAsync();
        Task<OdaTipleri?> SoftFirstOrDefaultAsync(int id);
        Task<List<OdaTipleriDto>> SoftGetRoomsByKonaklamaIdAsync(int konaklamaId);
        Task<bool> SoftAddAsync(OdaTipleri odaTipleri);
        Task<bool> SoftUpdateAsync(OdaTipleri odaTipleri);
        Task<bool> SoftDeleteAsync(int id);
    }
}
