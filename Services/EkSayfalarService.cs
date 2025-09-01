using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace dafsem.Services
{
    public class EkSayfalarService : IEkSayfalarService
    {
        private readonly AplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDilService _dilService;

        public EkSayfalarService(AplicationDbContext context, IWebHostEnvironment env, IDilService dilService)
        {
            _context = context;
            _env = env;
            _dilService = dilService;
        }
        public async Task<List<EkSayfalar>> SoftGetAllSayfalarAsync()
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.EkSayfalar.AsNoTracking()
                .Where(ek => ek.State && ek.DilId == dilId)
                .Include(e => e.BulunduguSayfa)
                .ToListAsync();
        }
        public async Task<EkSayfalar?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.EkSayfalar
                .AsNoTracking()
                .Include(e => e.BulunduguSayfa)
                .Include(s => s.SayfaBilesenleri.Where(b => b.State))
                .ThenInclude(b => b.SayfaBilesenDegerleri.Where(d => d.State))
                .FirstOrDefaultAsync(s => s.Id == id && s.State);
        }
        public async Task<EkSayfalar?> SoftSimpleFirstOrDefaultAsync(int id)
        {
            return await _context.EkSayfalar
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id && m.State);
        }

        public async Task<SelectList> SoftGetAllAsSelectList(int? selectedId = null)
        {
            try
            {
                int dilId = await _dilService.SoftGetDilIdFromCookie();

                var ekSayfalar = await _context.EkSayfalar
                    .AsNoTracking()
                    .Where(e => e.State && e.DilId == dilId)
                    .Select(e => new { e.Id, e.Url })
                    .ToListAsync();

                return new SelectList(ekSayfalar, "Id", "Url", selectedId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }
        }


        public async Task<List<BilesenlerDto>?> SoftGetAllBilesenlerAsync()
        {
            var filePath = Path.Combine(_env.WebRootPath, "data", "bilesenler.json");

            if (!System.IO.File.Exists(filePath))
            {
                throw new DirectoryNotFoundException("Bileşenler bulunamadı");
            }

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var bilesenler = JsonSerializer.Deserialize<List<BilesenlerDto>>(json);

            return bilesenler;
        }

        public async Task<BilesenlerDto?> SoftBilesenFirstOrDefaultAsync(int id)
        {
            var model = await SoftGetAllBilesenlerAsync();
            var bilesen = model?.FirstOrDefault(b => b.Id == id);
            return bilesen;
        }

        public async Task<List<SayfaBilesen>?> SoftGetSayfaBilesenBySayfaId(int id)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.SayfaBilesen
                .AsNoTracking()
                .Where(sb => sb.EkSayfaId == id && sb.State && sb.DilId == dilId)
                .OrderBy(sb => sb.Sira)
                .ToListAsync();
        }

        public async Task<List<SayfaBilesenDegerleri>?> SoftGetDegerlerByBilesenId(int id)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.SayfaBilesenDegerleri
                .AsNoTracking()
                .Where(sbd => sbd.SayfaBilesenId == id && sbd.State && sbd.DilId == dilId)
                .ToListAsync();
        }

        public async Task<bool> SoftAddAsync(EksayfaViewModel model)
        {
            try
            {
                int dilId = await _dilService.SoftGetDilIdFromCookie();
                // 1️⃣ Yeni Sayfa oluştur
                EkSayfalar eklenenKayit = new EkSayfalar()
                {
                    SayfaBasligi = model.SayfaBasligi,
                    Url = model.Url,
                    BulunduguSayfaId = model.BulunduguSayfaId,
                    DilId = dilId,
                    State = true
                };

                await _context.EkSayfalar.AddAsync(eklenenKayit);
                await _context.SaveChangesAsync(); // Önce kaydet ki ID oluşsun

                int sira = 0;
                var ekler = model.Ekler;

                foreach (var ek in ekler)
                {
                    // 2️⃣ Sayfa Bileşeni oluştur
                    SayfaBilesen sayfaBilesen = new SayfaBilesen()
                    {
                        EkSayfaId = eklenenKayit.Id, // ❗ Foreign Key için ID kullan
                        Sira = sira++,
                        BilesenId = ek.DataIds[0],
                        DilId = dilId,
                        State = true
                    };

                    sayfaBilesen.DilId = await _dilService.SoftGetDilIdFromCookie();

                    await _context.SayfaBilesen.AddAsync(sayfaBilesen);
                    await _context.SaveChangesAsync(); // ID oluşsun

                    List<SayfaBilesenDegerleri> degerlerList = new List<SayfaBilesenDegerleri>();

                    for (int i = 0; i < ek.Ids.Count; i++)
                    {
                        var baslik = ek.Ids[i];
                        var deger = ek.Values[i];

                        SayfaBilesenDegerleri tmp = new SayfaBilesenDegerleri()
                        {
                            Baslik = baslik,
                            Deger = deger,
                            SayfaBilesenId = sayfaBilesen.Id, // ❗ Foreign Key için ID kullan
                            DilId = dilId,
                            State = true
                        };

                        degerlerList.Add(tmp);
                    }

                    // 3️⃣ Sayfa Bileşen Değerlerini ekleyelim
                    await _context.SayfaBilesenDegerleri.AddRangeAsync(degerlerList);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<EkSayfalarEditViewModel> GetEkSayfalarForEdit(int id)
        {
            // Ana sayfayı ve ilişkili bileşenleri çek
            var ekSayfa = await SoftSimpleFirstOrDefaultAsync((int)id); ;
            if (ekSayfa == null)
                throw new DirectoryNotFoundException("İstenilen sayfa bulunamadı");

            // İlgili sayfanın bileşenlerini çek
            var sayfaBilesenler = await SoftGetSayfaBilesenBySayfaId((int)id);

            // Her bileşenin değerlerini çek ve model hazırla
            var bilesenModelList = new List<BilesenViewModel>();

            foreach (var bilesen in sayfaBilesenler)
            {
                // Bileşenin değerlerini çek
                var bilesenDegerleri = await SoftGetDegerlerByBilesenId(bilesen.Id);

                // Bileşen tipini al
                var bilesenTipi = await SoftBilesenFirstOrDefaultAsync((int)bilesen.BilesenId);

                // Bileşen modeli oluştur
                var bilesenModel = new BilesenViewModel
                {
                    Index = bilesen.Sira,
                    DataId = bilesenTipi.Id,
                    Baslik = bilesenTipi.Baslik,
                    Content = bilesenTipi.Icerik,
                    SayfaBilesenId = bilesen.Id,
                    Values = bilesenDegerleri.Select(bd => bd.Deger).ToList(),
                    Ids = bilesenDegerleri.Select(bd => bd.Baslik).ToList(),
                    DataIds = new List<int> { bilesenTipi.Id }
                };

                bilesenModelList.Add(bilesenModel);
            }

            // View modelini hazırla
            var viewModel = new EkSayfalarEditViewModel
            {
                Id = ekSayfa.Id,
                SayfaBasligi = ekSayfa.SayfaBasligi,
                Url = ekSayfa.Url,
                BulunduguSayfaId = ekSayfa.BulunduguSayfaId,
                Bilesenler = bilesenModelList
            };


            return viewModel;
        }

        public async Task<bool> SoftEditAsync(EksayfaViewModel model)
        {
            int id = Convert.ToInt32(model.Id);

            var ekSayfa = await SoftSimpleFirstOrDefaultAsync(id);
            if (ekSayfa == null || ekSayfa.State == false)
            {
                throw new DirectoryNotFoundException("İstenilen sayfa bulunamadı");
            }

            int dilId = await _dilService.SoftGetDilIdFromCookie();

            // 1. Sayfa bilgilerini güncelle
            ekSayfa.SayfaBasligi = model.SayfaBasligi;
            ekSayfa.Url = model.Url;
            ekSayfa.BulunduguSayfaId = model.BulunduguSayfaId;
            ekSayfa.DilId = dilId;

            // 2. Mevcut sayfa bileşenlerini al
            var mevcutBilesenler = await SoftGetSayfaBilesenBySayfaId(id);

            // 3. Mevcut bileşenleri pasif yap (soft delete)
            foreach (var bilesen in mevcutBilesenler)
            {
                bilesen.State = false;
                _context.SayfaBilesen.Update(bilesen);

                // İlişkili değerleri de pasif yap
                var bilesenDegerleri = await SoftGetDegerlerByBilesenId(bilesen.Id);

                foreach (var deger in bilesenDegerleri)
                {
                    deger.State = false;
                    _context.SayfaBilesenDegerleri.Update(deger);
                }
            }

            // 4. Yeni bileşenleri ekle
            int sira = 0;
            if (model.Ekler != null && model.Ekler.Count > 0)
            {
                foreach (var ek in model.Ekler)
                {
                    // Yeni Sayfa Bileşeni oluştur
                    SayfaBilesen sayfaBilesen = new SayfaBilesen()
                    {
                        EkSayfaId = id,
                        Sira = sira++,
                        BilesenId = ek.DataIds[0],
                        DilId = dilId,
                        State = true
                    };

                    await _context.SayfaBilesen.AddAsync(sayfaBilesen);
                    await _context.SaveChangesAsync(); // ID oluşsun

                    List<SayfaBilesenDegerleri> degerlerList = new List<SayfaBilesenDegerleri>();

                    for (int i = 0; i < ek.Ids.Count; i++)
                    {
                        var baslik = ek.Ids[i];
                        var deger = ek.Values[i];

                        SayfaBilesenDegerleri tmp = new SayfaBilesenDegerleri()
                        {
                            Baslik = baslik,
                            Deger = deger,
                            SayfaBilesenId = sayfaBilesen.Id,
                            DilId = dilId,
                            State = true
                        };

                        degerlerList.Add(tmp);
                    }

                    // Sayfa Bileşen Değerlerini ekleyelim
                    await _context.SayfaBilesenDegerleri.AddRangeAsync(degerlerList);
                }
            }

            _context.EkSayfalar.Update(ekSayfa);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<EkSayfalar?> SoftFind(int id)
        {
            int dilId = await _dilService.SoftGetDilIdFromCookie();

            return await _context.EkSayfalar
                .Include(e => e.BulunduguSayfa)
               .Include(s => s.SayfaBilesenleri.Where(b => b.State && b.DilId == dilId))
               .ThenInclude(b => b.SayfaBilesenDegerleri.Where(d => d.State && d.DilId == dilId))
               .FirstOrDefaultAsync(s => s.Id == id && s.State && s.DilId == dilId);
        }
        public async Task<bool> SoftDeleteAsync(int id)
        {
            EkSayfalar? sayfa = await SoftFind(id);
            if (sayfa == null)
                return true;

            foreach (var sayfaBilesen in sayfa.SayfaBilesenleri)
            {
                foreach (var deger in sayfaBilesen.SayfaBilesenDegerleri)
                {
                    deger.State = false;
                }
                sayfaBilesen.State = false;
            }
            sayfa.State = false;


            await _context.SaveChangesAsync();
            return true;
        }
    }
}
