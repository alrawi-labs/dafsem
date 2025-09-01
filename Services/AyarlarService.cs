using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Services
{
    public class AyarlarService : IAyarlarService
    {
        private readonly AplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly SayfaService _sayfaService;
        private readonly IDilService _dilService;

        public AyarlarService(AplicationDbContext context, IFileService fileService, SayfaService sayfaService, IDilService dilService)
        {
            _context = context;
            _fileService = fileService;
            _sayfaService = sayfaService;
            _dilService = dilService;
        }
        private async Task UpdateFotografAsync<T>(T model, IFormFile? newFotograf, string propertyName) where T : class
        {
            if (model == null || newFotograf == null) return;

            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException($"'{propertyName}' özelliği {typeof(T).Name} sınıfında bulunamadı.");


            // Fotoğraf güncellenmediğinde tekrar kaydetmeme gerek yok

            // Eski dosyayı sil ve yeni dosyayı yükle
            var oldFoto = property.GetValue(model) as Fotolar;
            if (oldFoto != null)
            {
                //await _fileService.DeleteFileAsync(oldFoto.Yol!);
                //_context.Fotolar.Remove(oldFoto);
            }
            try
            {
                int dilId = await _dilService.SoftGetDilIdFromCookie();

                var newFilePath = await _fileService.UploadFileAsync(newFotograf, "/Uploads/Site/Logolar", FileService.DosyaTuru.Photo);
                var newFoto = new Fotolar { Yol = newFilePath, DilId = dilId };

                property.SetValue(model, newFoto);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }

        private async Task UpdateDosyaAsync<T>(T model, IFormFile? dosya, string propertyName) where T : class
        {
            if (model == null) return;

            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException($"'{propertyName}' özelliği {typeof(T).Name} sınıfında bulunamadı.");

            if (dosya == null)
            {
                int dilId = await _dilService.SoftGetDilIdFromCookie();
                var existingFile = await _context.Ayarlar
                    .Where(x => x.Program != null && x.State && x.DilId == dilId)
                    .Select(x => x.Program)
                    .SingleOrDefaultAsync();

                property.SetValue(model, existingFile);
                return;
            }

            // Eski dosyayı sil ve yeni dosyayı yükle
            var oldFilePath = property.GetValue(model) as string;
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                //await _fileService.DeleteFileAsync(oldFilePath);
            }

            var newFilePath = await _fileService.UploadFileAsync(dosya, "/Uploads/Site/Docs", FileService.DosyaTuru.Belge);
            property.SetValue(model, newFilePath);
        }

        public async Task<Ayarlar> SoftGetAyarlarAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var ayarlarQuery = _context.Ayarlar
                .AsNoTracking()
                .OrderByDescending(i => i.Id)
                .Include(x => x.SiteLogo)
                .Include(x => x.Sayfalar.Where(s => s.State && s.DilId == dilId)) // Include içinde Where çalışmaz!
                    .ThenInclude(y => y.AltSayfalari.Where(a => a.State && a.DilId == dilId))
                .Include(x => x.SagLogo)
                .Include(x => x.SolLogo)
                .Include(x => x.Filigran)
                .Include(x => x.SiteArkaplani);

            // Önce State = true olanı arıyoruz
            var ayarlar = await ayarlarQuery.Where(i => i.State && i.DilId == dilId).FirstOrDefaultAsync();

            if (ayarlar == null)
                ayarlar = await ayarlarQuery.FirstOrDefaultAsync(i=> i.DilId == dilId);

            // Eğer hiç aktif ayar yoksa, tüm ayarlar arasından en sonuncusunu al


            if (ayarlar != null)
            {
                ayarlar.DilId = dilId;
                //ayarlar.Sayfalar = await SoftGetSayfalar();
                return ayarlar;
            }

            // Eğer hiç ayar yoksa, yeni bir tane oluştur
            ayarlar = new Ayarlar();
            ayarlar.DilId = dilId;
            await _context.Ayarlar.AddAsync(ayarlar);
            await _context.SaveChangesAsync();
            ayarlar.Sayfalar = await SoftGetSayfalar();
            _context.Ayarlar.Update(ayarlar);
            //await _context.SaveChangesAsync();
            return ayarlar;
        }


        private async Task<ICollection<Sayfalar>> SoftGetSayfalar()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            // Sayfalar ve AltSayfalar ilişkisini birlikte yükleme
            var sayfalar = await _context.Sayfalar
                .AsNoTracking()
                .Where(s => s.State && s.DilId == dilId)
                .Include(s => s.AltSayfalari.Where(a => a.State && a.DilId == dilId)) // AltSayfalar'ı dahil et
                .ToListAsync();

            if (sayfalar.Count() == 0)
            {
                await _sayfaService?.SayfalariSifirla();
                sayfalar = await _context.Sayfalar
                .AsNoTracking()
                .Where(s => s.State && s.DilId == dilId)
                .Include(s => s.AltSayfalari.Where(a => a.State && a.DilId == dilId)) // AltSayfalar'ı dahil et
                .ToListAsync();
            }

            return sayfalar;
        }

        public async Task<bool> SoftDeletePhotoAsync(string prp)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var ayarlar = await _context.Ayarlar.Where(a => a.State && a.DilId == dilId)
                .OrderByDescending(i => i.Id).FirstOrDefaultAsync();

            var foto = typeof(Ayarlar).GetProperty(prp);
            if (foto != null)
            {
                foto.SetValue(ayarlar, null);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> SoftDeleteFileAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            var ayarlar = await _context.Ayarlar.Where(a => a.State && a.DilId == dilId).FirstOrDefaultAsync();
            if (ayarlar == null || string.IsNullOrEmpty(ayarlar.Program)) return false;

            //await _fileService.DeleteFileAsync(ayarlar.Program);
            ayarlar.Program = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftUpdateAsync(Ayarlar ayarlar, IFormFile? program, IFormFile? siteLogo, IFormFile? sagLogo, IFormFile? solLogo, IFormFile? filigran, IFormFile? siteArkaplani)
        {

            Ayarlar model = await SoftGetAyarlarAsync();
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            if (model == null)
            {
                ayarlar.DilId = dilId;
                await _context.Ayarlar.AddAsync(ayarlar);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Yedek için bir kayıt eklemek
                Ayarlar yedek = new Ayarlar() // bunu Fahrettin hocama sormam gerek eğer hepsini kaydeedilmesine gerek yok diyorsa silmem kafi olur
                {
                    SiteAdi = model.SiteAdi,
                    KurumAdi = model.KurumAdi,
                    Filigran = model.Filigran != null ? new Fotolar { Yol = model.Filigran.Yol, State = false } : null,
                    SolLogo = model.SolLogo != null ? new Fotolar { Yol = model.SolLogo.Yol, State = false } : null,
                    Program = model.Program,
                    SiteLogo = model.SiteLogo != null ? new Fotolar { Yol = model.SiteLogo.Yol, State = false } : null,
                    SagLogo = model.SagLogo != null ? new Fotolar { Yol = model.SagLogo.Yol, State = false } : null,
                    SiteAltBaslik = model.SiteAltBaslik,
                    SiteArkaplani = model.SiteArkaplani != null ? new Fotolar { Yol = model.SiteArkaplani.Yol, State = false } : null,
                    // İlişkili koleksiyonları klonlayarak yeni instance'lar oluşturuyoruz
                    Sayfalar = model.Sayfalar?.Select(s => new Sayfalar
                    {
                        // Id'yi kopyalamıyoruz, EF Core otomatik oluşturacak
                        SayfaBasligi = s.SayfaBasligi,
                        Url = s.Url,
                        State = false,
                        // AltSayfalari için de benzer şekilde klonlama yapıyoruz
                        AltSayfalari = s.AltSayfalari?.Select(a => new AltSayfa
                        {
                            // Id'yi kopyalamıyoruz
                            AltSayfaBaslik = a.AltSayfaBaslik,
                            Url = a.Url,
                            State = false
                        }).ToList()
                    }).ToList(),
                    DilId = (model.DilId != 0) ? model.DilId : dilId,
                    State = false
                };

                ayarlar.DilId = (model?.DilId != 0) ? model.DilId : dilId;
                await _context.Ayarlar.AddAsync(yedek);
            }



            if (siteLogo != null)
                await UpdateFotografAsync(ayarlar, siteLogo, nameof(ayarlar.SiteLogo));
            else
                ayarlar.SiteLogoId = model?.SiteLogoId;

            if (sagLogo != null)
                await UpdateFotografAsync(ayarlar, sagLogo, nameof(ayarlar.SagLogo));
            else
                ayarlar.SagLogoId = model?.SagLogoId;

            if (solLogo != null)
                await UpdateFotografAsync(ayarlar, solLogo, nameof(ayarlar.SolLogo));
            else
                ayarlar.SolLogoId = model?.SolLogoId;

            if (filigran != null)
                await UpdateFotografAsync(ayarlar, filigran, nameof(ayarlar.Filigran));
            else
                ayarlar.FiligranId = model?.FiligranId;

            if (siteArkaplani != null)
                await UpdateFotografAsync(ayarlar, siteArkaplani, nameof(ayarlar.SiteArkaplani));
            else
                ayarlar.SiteArkaplaniId = model?.SiteArkaplaniId;

            if (program != null)
                await UpdateDosyaAsync(ayarlar, program, nameof(ayarlar.Program));
            else
                ayarlar.Program = model?.Program;

            _context.Update(ayarlar);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
