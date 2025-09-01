using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Models.ViewModels.UserSite;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace dafsem.Services
{
    public class HomeService : IHomeService
    {
        private readonly AplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeService(AplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // İlk açıldığında hiçbir dil olamdığından dolayı error sayfasına yönlendirilecektir,
        // Bu sorunun üstesinden gelinebilirdi, TurkceKontrolu metodu buraya yazar ve altaki
        // metotta çalıştırabilirdim ancak bunu yapmamamın sebebi, IHomeService içinde hiçbir
        // yazma işlemini gerçekleşemesini istediğimden, insert yapabilmek için admin hesabı
        // olmak zorunludur.
        public async Task<int> GetIdbyDilKodu(string dilKodu)
        {
            return await _context.Dil
                    .Where(d => d.DilKodu == dilKodu && d.State)
                    .Select(d => d.Id).FirstOrDefaultAsync();
        }

        public async Task<AnaSayfaViewModel?> GetAnaSayfa(int dilId)
        {
            return await _context.AnaSayfa
            .AsNoTracking()
            .Where(a => a.State && a.DilId == dilId)
            .Select(a => new AnaSayfaViewModel
            {
                // Sadece ihtiyaç duyulan ana alanlar
                Mektup = a.Mektup,
                AfisYolu = a.AfiseId.Yol,
                SliderYollari = a.Sliderler
                    .Where(s => s.State && s.DilId == dilId)
                    .Select(s => s.Yol)
                    .ToList()
            })
             .FirstOrDefaultAsync();
        }

        public async Task<List<TarihlerViewModel>> GetTarihler(int dilId)
        {
            var model = await _context.Tarihler
                .AsNoTracking()
                .Include(t => t.Dil)
                .Where(t => t.State && t.DilId == dilId)
                .Select(t => new TarihlerViewModel
                {
                    Yil = t.Tarih.Year,
                    GunAy = t.Tarih.ToString("dd MMMM", new System.Globalization.CultureInfo($"{t.Dil.DilKodu.ToLower()}-{t.Dil.DilKodu.ToUpper()}")).ToUpper(),
                    // birden fazla dil olunca kodu ondan istenir (tr-TR) olan ve oraya bir link bırakırım ondan bu kodu alabilecek
                    Aciklama = t.Aciklama
                })
                .ToListAsync();

            return model;
        }

        public async Task<string> GetTitleOfSayfa(string SayfaUrl, int dilId)
        {
            return await _context.Sayfalar
                .AsNoTracking()
                .Where(t => t.Url == SayfaUrl && t.State && t.DilId == dilId)
                .Select(t => t.SayfaBasligi)
                .FirstOrDefaultAsync() ?? "";
        }

        public async Task<List<KategoriUyelerViewModel>> GetKategoriUyeler(string SayfaUrl, int dilId)
        {
            string dilKodu = await _context.Dil.Where(d => d.State && d.Id == dilId).Select(d => d.DilKodu).FirstOrDefaultAsync() ?? "tr";

            var model = await _context.KurulKategorileri
               .AsNoTracking()
               .Include(k => k.KurulUyeleri.Where(ku => ku.State && ku.DilId == dilId))!
                   .ThenInclude(u => u.Unvan)
               .Where(k => k.Sayfa.Url == SayfaUrl && k.State && k.DilId == dilId)
               .Select(k => new KategoriUyelerViewModel
               {
                   Kategori = k.Baslik.ToUpper(new System.Globalization.CultureInfo($"{dilKodu.ToLower()}-{dilKodu.ToUpper()}")),
                   Uye = k.KurulUyeleri
                   .Where(ku => ku.State && ku.DilId == dilId)
                   .OrderBy(u => u.Unvan == null ? 1 : 0) // Ünvansız olanları en sona al
                   .ThenBy(u => u.Unvan != null ? u.Unvan.Sira : int.MaxValue) // Unvanlıları 'Sira'ya göre sırala
                   .ThenBy(u => u.Adi) // Aynı 'Sira' değerine sahip olanları adıyla alfabetik sırala
                   .ThenBy(u => u.Soyadi) // Eğer 'Adi' aynıysa, soyada göre alfabetik sırala
                   .Select(u => $"{(u.Unvan.UnvanAdi)} {u.Adi} {u.Soyadi.ToUpper(new System.Globalization.CultureInfo($"{dilKodu.ToLower()}-{dilKodu.ToUpper()}"))} ({u.Kurum})") // eğer ünvan yoksa hata beklenir (kontrol et)
                   .ToList()
               })
               .ToListAsync();

            return model;
        }

        public async Task<string> GetTitleOfAltSayfa(string SayfaUrl, int dilId)
        {
            return await _context.AltSayfa
                .AsNoTracking()
                .Where(t => t.Url == SayfaUrl && t.State && t.DilId == dilId)
                .Select(t => t.AltSayfaBaslik)
                .FirstOrDefaultAsync() ?? "";
        }

        public async Task<List<KategoriUyelerViewModel>> GetBasliklar(int dilId)
        {
            return new List<KategoriUyelerViewModel> // Liste oluşturuldu
            {
                new KategoriUyelerViewModel
                {
                    Uye = await _context.Basliklar
                    .AsNoTracking()
                    .Where(b => b.State && b.DilId == dilId)
                    .Select(b => b.Baslik)
                    .ToListAsync()
                }
            };
        }

        public async Task<BasvuruViewModel?> GetBasvuru(int dilId)
        {
            string mail = await _context.Iletisim
                .AsNoTracking()
                .Where(i => i.State && i.DilId == dilId)
                .Select(a => a.Eposta)
                .FirstOrDefaultAsync() ?? "";

            return await _context.Basvuru
                .AsNoTracking()
                .Where(b => b.State && b.DilId == dilId)
                .Select(b => new BasvuruViewModel
                {
                    AltMetin = b.AltMetin,
                    Form = b.Form,
                    UstMetin = b.UstMetin,
                    EPosta = mail
                }).FirstOrDefaultAsync();
        }

        public async Task<IletisimViewModel?> GetIletisim(int dilId)
        {
            return await _context.Iletisim
                .AsNoTracking()
                .Where(i => i.State && i.DilId == dilId)
                .Include(t => t.Telefonlar.Where(t => t.State && t.DilId == dilId))
                .Select(i => new IletisimViewModel
                {
                    Eposta = i.Eposta,
                    Adres = i.Adres,
                    Telefonlar = i.Telefonlar
                }).FirstOrDefaultAsync();
        }

        public async Task<List<KonaklamaViewModel>> GetKonaklama(int dilId)
        {
            return await _context.Konaklama
                   .AsNoTracking()
                   .Where(k => k.State && k.DilId == dilId)
                   .Include(k => k.Odalar)
                   .Select(k => new KonaklamaViewModel
                   {
                       Adres = k.Adres,
                       KonaklamaEvi = k.KonaklamaEvi,
                       Eposta = k.Eposta,
                       KahvaltiDahilMi = (k.KahvaltiDahilMi == null ? null : k.KahvaltiDahilMi == true ? "Evet" : "Hayır"),
                       Tel = k.Tel,
                       WebSitesi = k.WebSitesi,
                       YildizSayisi = k.YildizSayisi,
                       Odalar = k.Odalar.Select(o => $"{o.OdaTipi}: {o.Ucret} {o.Birim!.Sembol}")
                   }).ToListAsync();
        }

        public async Task<string[]> GetKurallar(string SayfaUrl, int dilId)
        {
            return await _context.Kurallar
                .AsNoTracking()
                .Where(k => k.Turu.Sayfa.Url == SayfaUrl && k.State && k.DilId == dilId)
                .Select(k => k.Metin)
                .ToArrayAsync();
        }

        public async Task<string?> GetProgram(int dilId)
        {
            return await _context.Ayarlar
                 .AsNoTracking()
                 .Where(a => a.State && a.DilId == dilId)
                 .Select(a => a.Program)
                 .FirstOrDefaultAsync();
        }

        public async Task<UcretHizmetBankaViewModel?> GetUcretHizmetBanka(int dilId)
        {
            return new UcretHizmetBankaViewModel
            {
                Ucretler = await _context.Ucretler
                .AsNoTracking()
                .Where(u => u.State && u.DilId == dilId)
                    .Select(u => new UcretViewModel
                    {
                        Baslik = u.Baslik,
                        Ucret = $"{u.Ucret} {u.Birim.Sembol}"
                    })
                    .ToListAsync(),

                HizmetTurler = await _context.HizmetTuru
                .AsNoTracking()
                .Where(h => h.State && h.DilId == dilId)
                    .Select(t => new HizmetTuruViewModel
                    {
                        Tur = t.Tur,
                        Hizmetler = _context.Hizmetler // senkron bir şekilde olduğu için hata verebilir, kontrol et
                            .AsNoTracking()
                            .Where(h => h.Turu.Id == t.Id && t.State && t.DilId == dilId)
                            .Select(h => h.Hizmet)
                            .ToList()
                    })
                    .ToListAsync(),

                Bankalar = await _context.BankaBilgileri
                .AsNoTracking()
                .Where(b => b.State && b.DilId == dilId)
                    .Select(b => new BankaViewModel
                    {
                        BankaAdi = b.BankaAdi,
                        HesapSahibiAdi = b.HesapSahibiAdi,
                        IBAN = b.IBAN
                    })
                    .ToListAsync() // Better than SingleOrDefault for potential multiple records
            };
        }

        public async Task<SayfaDetayViewModel> GetEkSayfa(string url)
        {
            var sayfa = await _context.EkSayfalar
                .AsNoTracking()
                .Include(e => e.BulunduguSayfa)
                .Include(s => s.SayfaBilesenleri.Where(b => b.State))
                .ThenInclude(b => b.SayfaBilesenDegerleri.Where(d => d.State))
                .FirstOrDefaultAsync(s => s.Url == url && s.State);

            if (sayfa == null)
                throw new Exception("Sayfa bulunamamaktadır");

            var filePath = Path.Combine(_env.WebRootPath, "data", "bilesenler.json");

            if (!System.IO.File.Exists(filePath))
            {
                throw new DirectoryNotFoundException("Bileşenler bulunamadı");
            }

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var bilesenler = JsonSerializer.Deserialize<List<BilesenlerDto>>(json);
            // Bileşenleri ayrı çekiyoruz

            var viewModel = new SayfaDetayViewModel
            {
                Sayfa = sayfa,
                Bilesenler = bilesenler
            };

            return viewModel;
        }
    }
}
