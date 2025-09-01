using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IAyarlarService
    {
        Task<Ayarlar> SoftGetAyarlarAsync();
        Task<bool> SoftUpdateAsync(Ayarlar ayarlar, IFormFile? program, IFormFile? siteLogo, IFormFile? sagLogo, IFormFile? solLogo, IFormFile? filigran, IFormFile? siteArkaplani); 
        Task<bool> SoftDeletePhotoAsync(string id);
        Task<bool> SoftDeleteFileAsync();

    }
}
