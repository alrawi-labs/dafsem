using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IAnaSayfaService
    {
        Task<AnaSayfa?> SoftGetAnaSayfaAsync();
        Task<bool> SoftUpdateAsync(AnaSayfa model, IFormFile? Afise);
        Task<Fotolar> SoftAddSliderAsync(IFormFile SliderPhoto);
        Task<bool> SoftDeleteAfisAsync();
        Task<bool> SoftDeleteFotoAsync(int id);
    }
}
