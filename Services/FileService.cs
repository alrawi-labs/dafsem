using dafsem.Services.Contracts;

namespace dafsem.Services
{
    public class FileService : IFileService
    {
        private const string ROOT = "wwwroot"; // Dosyaların kaydedileceği kök dizin

        public enum DosyaTuru
        {
            Photo,
            Belge,
            Doc,
            File
        }

        // Desteklenen dosya uzantıları
        private readonly Dictionary<DosyaTuru, HashSet<string>> AllowedExtensions = new()
        {
            { DosyaTuru.Photo, new HashSet<string> { ".jpg", ".png", ".jpeg" } },
            { DosyaTuru.Belge,  new HashSet<string> { ".pdf"} },
            { DosyaTuru.Doc,  new HashSet<string> { ".pdf",".doc", ".docx" } },
            { DosyaTuru.File,  new HashSet<string> { ".txt", ".csv", ".xlsx" } }
        };

        // Maksimum dosya boyutları (bayt cinsinden)
        private readonly Dictionary<DosyaTuru, int> MaxFileSizes = new()
        {
            { DosyaTuru.Photo, 3 * 1024 * 1024 },  // 3 MB
            { DosyaTuru.Belge,  10 * 1024 * 1024 }, // 10 MB
            { DosyaTuru.Doc,  10 * 1024 * 1024 }, // 10 MB
            { DosyaTuru.File,  7 * 1024 * 1024 }   // 7 MB
        };

        /// <summary>
        /// Belirtilen dosya türüne göre kabul edilen dosya uzantılarını döndürür.
        /// </summary>
        /// <param name="dosyaTuru">Dosya türü</param>
        /// <returns>Uzantıları içeren string (örn: ".jpg, .png")</returns>
        /// <exception cref="ArgumentException">Geçersiz dosya türü girildiğinde hata fırlatır.</exception>
        public string GetAcceptString(DosyaTuru dosyaTuru)
        {
            if (AllowedExtensions.TryGetValue(dosyaTuru, out var extensions))
            {
                return string.Join(", ", extensions);
            }
            throw new ArgumentException("Geçersiz dosya türü", nameof(dosyaTuru));
        }

        /// <summary>
        /// Belirtilen dosya türü için maksimum dosya boyutunu döndürür.
        /// </summary>
        /// <param name="dosyaTuru">Dosya türü</param>
        /// <returns>Maksimum dosya boyutu (bayt cinsinden)</returns>
        /// <exception cref="ArgumentException">Geçersiz dosya türü girildiğinde hata fırlatır.</exception>
        public int GetMaxFileSize(DosyaTuru dosyaTuru)
        {
            if (MaxFileSizes.TryGetValue(dosyaTuru, out var maxSize))
            {
                return maxSize;
            }
            throw new ArgumentException("Geçersiz dosya türü", nameof(dosyaTuru));
        }

        /// <summary>
        /// Dosya yükleme işlemini gerçekleştirir.
        /// </summary>
        /// <param name="file">Yüklenecek dosya</param>
        /// <param name="directory">Dosyanın kaydedileceği dizin</param>
        /// <param name="dosyaTuru">Dosyanın türü (Photo, Form, File)</param>
        /// <returns>Yüklenen dosyanın yolunu döndürür</returns>
        /// <exception cref="ArgumentException">Geçersiz dosya türü veya uzantısı</exception>
        /// <exception cref="InvalidOperationException">Dosya boyutu sınırı aşıldığında</exception>
        public async Task<string> UploadFileAsync(IFormFile file, string directory, DosyaTuru dosyaTuru)
        {
            if (!AllowedExtensions.TryGetValue(dosyaTuru, out var validExtensions))
                throw new ArgumentException("Geçersiz dosya türü", nameof(dosyaTuru));

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            // Uzantı kontrolü
            if (!validExtensions.Contains(fileExtension))
                throw new InvalidOperationException(
                    $"Desteklenmeyen dosya biçimi: '{fileExtension}'. " +
                    $"İzin verilen formatlar: {GetAcceptString(dosyaTuru)}");

            // Maksimum dosya boyutu kontrolü
            int maxFileSize = GetMaxFileSize(dosyaTuru);
            if (file.Length > maxFileSize)
                throw new InvalidOperationException($"Dosya boyutu çok büyük: {file.Length} byte. Maksimum izin verilen: {maxFileSize} byte.");

            // Dosya adı oluşturma
            var randomFileName = Guid.NewGuid().ToString() + fileExtension;
            var uploadPath = Path.Combine(directory, randomFileName);

            // Dosyayı kaydetme işlemi
            using (var fileStream = new FileStream(ROOT + uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uploadPath;
        }

        /// <summary>
        /// Belirtilen dosya yoluna göre dosya indirme işlemini gerçekleştirir.
        /// </summary>
        /// <param name="relativePath">wwwroot altındaki dosya yolu, örn: "Uploads/Site/Docs/Kurul-Uyeleri.xlsx"</param>
        /// <param name="fileName">Kullanıcıya gösterilecek dosya adı</param>
        /// <param name="mimeType">Dosyanın MIME türü</param>
        /// <returns>Dosya baytlarını, MIME türünü ve dosya adını içeren tuple</returns>
        public async Task<(byte[] fileBytes, string mimeType, string fileName)> DownloadFileAsync(string relativePath, string fileName, string mimeType)
        {
            // Tam dosya yolunu oluşturuyoruz
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), ROOT, relativePath);

            // Dosya varlığını kontrol et
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Dosya bulunamadı.", filePath);
            }

            // Dosyayı asenkron olarak bayt dizisine dönüştür
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return (fileBytes, mimeType, fileName);
        }
    }
}
