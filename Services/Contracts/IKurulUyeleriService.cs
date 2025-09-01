using dafsem.Models;
using dafsem.Models.ViewModels;

namespace dafsem.Services.Contracts
{
    public interface IKurulUyeleriService
    {
        Task<ICollection<KurulUyeleri>> SoftGetAllAsync();
        Task<KurulUyeleri?> SoftFirstOrDefaultAsync(int id);
        Task<byte[]> SoftWriteToExcel(List<KurulUyeleri> kurulUyeleri);
        Task<List<KurulUyeleri>[]> SoftReadExcelFileAsync(IFormFile excelFile);
        Task<(bool success, string message)> SoftExceldenKurulUyeleriEkle(ICollection<KurulUyeImportDto> excelData);
        Task<bool> SoftAddAsync(KurulUyeleri kurulUyeleri);
        Task<bool> SoftUpdateAsync(KurulUyeleri kurulUyeleri);
        Task<bool> SoftDeleteAsync(int id);
    }
}
