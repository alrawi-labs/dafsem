using dafsem.Models;
using dafsem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Services.Contracts
{
    public interface IEkSayfalarService
    {
        Task<List<EkSayfalar>> SoftGetAllSayfalarAsync();
        Task<EkSayfalar?> SoftFirstOrDefaultAsync(int id);
        Task<SelectList> SoftGetAllAsSelectList(int? selectedId = null);
        Task<List<BilesenlerDto>?> SoftGetAllBilesenlerAsync();
        Task<BilesenlerDto?> SoftBilesenFirstOrDefaultAsync(int id);
        Task<bool> SoftAddAsync(EksayfaViewModel model);
        Task<EkSayfalar?> SoftSimpleFirstOrDefaultAsync(int id);
        Task<List<SayfaBilesen>?> SoftGetSayfaBilesenBySayfaId(int id);
        Task<List<SayfaBilesenDegerleri>?> SoftGetDegerlerByBilesenId(int id);
        Task<EkSayfalarEditViewModel> GetEkSayfalarForEdit(int id);
        Task<bool> SoftEditAsync(EksayfaViewModel model);
        Task<bool> SoftDeleteAsync(int id);
        
    }
}
