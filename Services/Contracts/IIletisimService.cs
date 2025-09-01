using dafsem.Models;
using dafsem.Models.ViewModels;

namespace dafsem.Services.Contracts
{
    public interface IIletisimService
    {
        Task<Iletisim?> SoftGetLastAsync();
        Task<bool> SoftTelefonAddAsync(TelefonDto telefon);
        Task<bool> SoftUpdateAsync(Iletisim iletisim);
        Task<ICollection<Telefonlar>> SoftGetTelefonlar();
    }
}
