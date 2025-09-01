using dafsem.Models;
using dafsem.Models.ViewModels;

namespace dafsem.Services.Contracts
{
    public interface ITelefonlarService
    {
        Task<Telefonlar?> SoftFirstOrDefault(int id);
        Task<bool> SoftAddAsync(Telefonlar telefon);
        Task<bool> SoftUpdateAsync(int id, TelefonDto telefon);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> CheckValidTelefonAsync(TelefonDto telefon);
    }
}
