using dafsem.Models;

namespace dafsem.Services.Contracts
{
    public interface IBasvuruService
    {
        Task<Basvuru?> SoftGetLastAsync();
        Task<bool> SoftUpdateAsync(Basvuru basvuru,IFormFile? basvuruForm);
        Task<bool> SoftDeleteForm();

    }
}
