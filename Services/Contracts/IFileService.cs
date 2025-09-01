using static dafsem.Services.FileService;

namespace dafsem.Services.Contracts
{
    public interface IFileService
    {
        string GetAcceptString(DosyaTuru dosyaTuru);
        int GetMaxFileSize(DosyaTuru dosyaTuru);
        Task<string> UploadFileAsync(IFormFile file, string directory, DosyaTuru dosyaTuru);

        /// <summary>
        /// Belirtilen dosya yoluna göre dosya indirme işlemini gerçekleştirir.
        /// </summary>
        /// <param name="relativePath">wwwroot altındaki dosya yolu, örn: "Uploads/Site/Docs/Kurul-Uyeleri.xlsx"</param>
        /// <param name="fileName">Kullanıcıya gösterilecek dosya adı</param>
        /// <param name="mimeType">Dosyanın MIME türü</param>
        /// <returns>Dosya baytlarını, MIME türünü ve dosya adını içeren tuple</returns>
        Task<(byte[] fileBytes, string mimeType, string fileName)> DownloadFileAsync(string relativePath, string fileName, string mimeType);
    }
}
