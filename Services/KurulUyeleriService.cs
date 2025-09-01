using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace dafsem.Services
{
    public class KurulUyeleriService : IKurulUyeleriService
    {
        private readonly AplicationDbContext _context;
        private readonly IDilService _dilService;
        public KurulUyeleriService(AplicationDbContext context, IDilService dilService)
        {
            _context = context;
            _dilService = dilService;
        }

        public async Task<ICollection<KurulUyeleri>> SoftGetAllAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.KurulUyeleri
                .AsNoTracking()
                .Where(u => u.State && u.DilId == dilId)
                .Include(a => a.Unvan)
                .Include(a => a.KategoriId)
                .OrderBy(a => a.KategoriId.Sira == null ? int.MaxValue : a.KategoriId.Sira) // Önce kategori sırasına göre sıralama
                .ThenBy(a => a.Unvan.Sira == null ? int.MaxValue : a.Unvan.Sira) // Sonra unvan sırasına göre sıralama
                .ToListAsync();
        }

        public Task<KurulUyeleri?> SoftFirstOrDefaultAsync(int id)
        {
            return _context.KurulUyeleri
                .AsNoTracking()
                .Include(k => k.KategoriId)
                .Include(u => u.Unvan)
                .FirstOrDefaultAsync(u => u.Id == id && u.State);
        }

        public async Task<bool> SoftAddAsync(KurulUyeleri kurulUyeleri)
        {
            if (kurulUyeleri == null)
            {
                return false;
            }

            try
            {
                kurulUyeleri.DilId = await _dilService.SoftGetDilIdFromCookie();
                kurulUyeleri.State = true;

                // Eğer Unvan nesnesi halihazırda veritabanında varsa (Id > 0), onu attach ediyoruz.
                if (kurulUyeleri.Unvan != null && kurulUyeleri.Unvan.Id > 0)
                {
                    _context.Entry(kurulUyeleri.Unvan).State = EntityState.Unchanged;
                }

                // Eğer Kategori (navigation property olarak) veritabanında mevcutsa, onu attach ediyoruz.
                if (kurulUyeleri.KategoriId != null && kurulUyeleri.KategoriId.Id > 0)
                {
                    _context.Entry(kurulUyeleri.KategoriId).State = EntityState.Unchanged;
                }

                // Eğer varsa diğer navigation property'ler (örneğin, AltSayfa) için de benzer işlemler yapılabilir.

                await _context.KurulUyeleri.AddAsync(kurulUyeleri);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // İsteğe bağlı: Hatanın loglanması
                return false;
            }
            return true;
        }

        public async Task<bool> SoftUpdateAsync(KurulUyeleri updatedEntity)
        {
            // 1. Mevcut kaydı veritabanından çekiyoruz
            var existingEntity = await SoftFirstOrDefaultAsync(updatedEntity.Id);
            if (existingEntity == null)
                return false;

            // 2. Eski kaydın bir kopyasını arşivlemek istiyorsanız, yeni bir kayıt oluşturun.
            // Bu kayıt, mevcut kaydın geçmiş halini saklamak için eklenecek.
            var archiveEntity = new KurulUyeleri
            {
                // Identity sütununu kopyalamıyoruz; EF Core yeni bir ID atayacak.
                Adi = existingEntity.Adi,
                Soyadi = existingEntity.Soyadi,
                Kurum = existingEntity.Kurum,
                DilId = existingEntity.DilId,
                State = false, // Eski kaydı pasif yapıyoruz.
                               // Navigation property'ler: Mevcut veritabanı instance'larını kullanıyoruz.
                KategoriId = existingEntity.KategoriId != null
                                ? await _context.KurulKategorileri.FindAsync(existingEntity.KategoriId.Id)
                                : null,
                Unvan = existingEntity.Unvan != null
                                ? await _context.Unvan.FindAsync(existingEntity.Unvan.Id)
                                : null
            };

            // Arşiv kaydını ekliyoruz.
            await _context.KurulUyeleri.AddAsync(archiveEntity);

            // 3. Mevcut entity'yi context'ten detach ediyoruz,
            // böylece formdan gelen updatedEntity ile çakışma yaşamayız.
            _context.Entry(existingEntity).State = EntityState.Detached;

            // 4. Güncelleme yapılacak updatedEntity'nin navigation property'lerini, 
            // context'ten çekilen mevcut instance'larla yeniden eşleştiriyoruz.
            if (updatedEntity.KategoriId != null)
            {
                var kategori = await _context.KurulKategorileri.FindAsync(updatedEntity.KategoriId.Id);
                updatedEntity.KategoriId = kategori;
            }
            if (updatedEntity.Unvan != null)
            {
                var unvan = await _context.Unvan.FindAsync(updatedEntity.Unvan.Id);
                updatedEntity.Unvan = unvan;
            }

            updatedEntity.DilId = existingEntity.DilId;

            // 5. Updated entity'yi update olarak işaretliyoruz.
            _context.Entry(updatedEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        private async Task<KurulUyeleri?> SoftFindAsync(int id)
        {
            var kurulUyeleri = await _context.KurulUyeleri.FindAsync(id);
            return (kurulUyeleri != null && kurulUyeleri.State) ? kurulUyeleri : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            KurulUyeleri? kurulUyeleri = await SoftFindAsync(id);
            if (kurulUyeleri == null)
                return false;

            kurulUyeleri.State = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<KurulUyeleri>[]> SoftReadExcelFileAsync(IFormFile excelFile)
        {
            // Doğru ve hatalı verileri tutmak için iki liste
            List<KurulUyeleri>[] result = new List<KurulUyeleri>[2];
            result[0] = new List<KurulUyeleri>(); // Doğru veriler
            result[1] = new List<KurulUyeleri>(); // Hatalı veriler

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);
                stream.Position = 0; // Akışın başlangıcına dön

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    // Sayfa geçerli mi kontrolü
                    if (worksheet.Dimension == null)
                    {
                        throw new Exception("Geçerli bir sayfa bulunamadı.");
                    }
                    int dilId = await _dilService.SoftGetDilIdFromCookie();

                    // Başlık satırını atlayarak 2. satırdan okumaya başla
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        // Kategori işleme (5. sütun)
                        var kategoriAdi = worksheet.Cells[row, 5].Text.ToLower();
                        var kat = _context.KurulKategorileri
                            .FirstOrDefault(x => x.Baslik.ToLower() == kategoriAdi && x.State && x.DilId == dilId);

                        // Unvan işleme (1. sütun)
                        var unvanAdi = worksheet.Cells[row, 1].Text.ToLower();
                        var unvan = _context.Unvan
                            .FirstOrDefault(x => x.UnvanAdi.ToLower() == unvanAdi && x.State && x.DilId == dilId);

                        bool isNameValid = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text);
                        bool isFullNameValid = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text);

                        // Hata mesajları
                        string katMessage = string.IsNullOrWhiteSpace(kategoriAdi) ? "Kategori alanı boştur" : $"\"{kategoriAdi}\", Hatalı veya kategori mevcut değildir!";
                        string unvanMessage = string.IsNullOrWhiteSpace(unvanAdi) ? "" : $"\"{unvanAdi}\", Hatalı veya unvan mevcut değildir!";

                        var uye = new KurulUyeleri
                        {
                            Unvan = unvan ?? new Unvan { Id = 0, UnvanAdi = unvanMessage },
                            Adi = isNameValid ? worksheet.Cells[row, 2].Text : "Bulunamadı!",
                            Soyadi = isFullNameValid ? worksheet.Cells[row, 3].Text : "Bulunamadı!",
                            Kurum = worksheet.Cells[row, 4].Text,
                            KategoriId = kat ?? new KurulKategorileri { Id = 0, Baslik = katMessage, SayfaId = 0 }
                        };

                        // Eğer kategori veya unvan hatalıysa, veriyi hatalı listeye ekle
                        if (kat == null || !(isNameValid && isFullNameValid))
                        {
                            result[1].Add(uye);
                        }
                        else
                        {
                            result[0].Add(uye);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<byte[]> SoftWriteToExcel(List<KurulUyeleri> kurulUyeleri)
        {
            // EPPlus'ın lisans bağlamını belirleyelim (non-commercial kullanım için)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var memoryStream = new MemoryStream())
            {
                using (var package = new ExcelPackage(memoryStream))
                {
                    // Yeni bir çalışma sayfası ekleyelim
                    var worksheet = package.Workbook.Worksheets.Add("Kurul Uyeleri Hatali Kayitlar");

                    // Başlık satırını ekleyelim
                    worksheet.Cells[1, 1].Value = "Ünvan";
                    worksheet.Cells[1, 2].Value = "Adı";
                    worksheet.Cells[1, 3].Value = "Soyadı";
                    worksheet.Cells[1, 4].Value = "Kurum";
                    worksheet.Cells[1, 5].Value = "Kategori";

                    // Verileri yazmaya başlayalım, 2. satırdan başlıyoruz
                    int row = 2;
                    foreach (var uye in kurulUyeleri)
                    {
                        // Eğer Unvan, Kategori gibi navigasyon property'ler varsa
                        // bunların hangi property'sini göstereceğinize karar verin. Örneğin:
                        worksheet.Cells[row, 1].Value = uye.Unvan != null ? uye.Unvan.ToString() : "";
                        worksheet.Cells[row, 2].Value = uye.Adi;
                        worksheet.Cells[row, 3].Value = uye.Soyadi;
                        worksheet.Cells[row, 4].Value = uye.Kurum;
                        // KategoriId burada bir navigation property ise:
                        worksheet.Cells[row, 5].Value = uye.KategoriId?.Baslik ?? "Kategori alanı boştur";
                        row++;
                    }

                    // Kaydet (package.Save() işlemi memoryStream'i dolduracaktır)
                    package.Save();
                }
                // MemoryStream'deki veriyi bayt dizisi olarak döndürüyoruz.
                return memoryStream.ToArray();
            }
        }

        public async Task<(bool success, string message)> SoftExceldenKurulUyeleriEkle(ICollection<KurulUyeImportDto> excelData)
        {
            // Eğer gelen veri boşsa hata döndür
            if (excelData == null || !excelData.Any())
            {
                return (false, "Veri bulunamadı.");
            }

            var newUyeler = new List<KurulUyeleri>();

            foreach (var dto in excelData)
            {
                // Kategori doğrulaması: Eğer kategori bulunamazsa işlem iptal ediliyor
                var kategori = await _context.KurulKategorileri
                    .AsNoTracking()
                    .FirstOrDefaultAsync(k => k.Id == dto.KategoriId);

                if (kategori == null)
                {
                    return (false, $"Geçersiz kategori ID: {dto.KategoriId}");
                }
                int dilId = await _dilService.SoftGetDilIdFromCookie();

                // Unvanı ve kategori kaydını veritabanından çekiyoruz
                var unvan = await _context.Unvan
                    .FirstOrDefaultAsync(u => u.Id == dto.UnvanId && u.State && u.DilId == dilId);

                var kategoriNav = await _context.KurulKategorileri
                    .FirstOrDefaultAsync(k => k.Id == dto.KategoriId && k.State && k.DilId == dilId);

                var uye = new KurulUyeleri
                {
                    Adi = dto.Adi,
                    Soyadi = dto.Soyadi,
                    Kurum = dto.Kurum,
                    DilId = dilId,
                    State = true,
                    // Eğer modelinizde foreign key için navigation property kullanıyorsanız, 
                    // bunu ona göre ayarlayın. Örneğin:
                    KategoriId = kategoriNav,
                    Unvan = unvan
                };

                newUyeler.Add(uye);
            }

            await _context.KurulUyeleri.AddRangeAsync(newUyeler);
            await _context.SaveChangesAsync();

            return (true, "Kurul üyeleri başarıyla eklendi.");
        }
    }
}
