using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dafsem.Context;
using dafsem.Models;
using dafsem.Services;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;
using Microsoft.Extensions.Localization;
using dafsem.Resources;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class AyarlarController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly SayfaService _sayfaService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AyarlarController(IServiceManager serviceManager, SayfaService sayfaService, IStringLocalizer<SharedResource> localizer)
        {
            _serviceManager = serviceManager;
            _sayfaService = sayfaService;
            _localizer = localizer;
        }

        // GET: Ayarlar
        public async Task<IActionResult> Index()
        {
            var ayarlar = await _serviceManager.AyarlarService.SoftGetAyarlarAsync();
            return View(ayarlar);
        }

        // PanelLanguage değiştirme fonksiyonu - Düzeltilmiş versiyon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePanelLanguage(string panelLanguage)
        {
            if (!string.IsNullOrEmpty(panelLanguage))
            {
                // Cookie'yi doğru formatta oluştur (ASP.NET Core localization standardına uygun)
                var cookieValue = $"c={panelLanguage}|uic={panelLanguage}";

                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1),
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                };

                Response.Cookies.Append("PanelLanguage", cookieValue, option);

                var message = _localizer["LanguageChangedSuccessfully"];
                TempData["Success"] = message.Value;

                Console.WriteLine("***********************************");
                Console.WriteLine($"Dil değiştirildi: {panelLanguage}");
                Console.WriteLine($"Cookie değeri: {cookieValue}");
                Console.WriteLine("***********************************");
            }
            else
            {
                var localizedString = _localizer["InvalidLanguageSelection"];
                var errorMessage = localizedString.ResourceNotFound
                    ? "Geçersiz dil seçimi!"
                    : localizedString.Value;

                TempData["Error"] = errorMessage;

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Ayarlar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var ayarlar = await _serviceManager.AyarlarService.SoftGetAyarlarAsync();

            if (ayarlar == null)
                ayarlar = new Ayarlar();


            ViewBag.AcceptPhoto = _serviceManager.FileService.GetAcceptString(FileService.DosyaTuru.Photo);
            ViewBag.AcceptForm = _serviceManager.FileService.GetAcceptString(FileService.DosyaTuru.Belge);
            return View(ayarlar);
        }


        [HttpDelete("/Ayarlar/DeletePhoto/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(string id)
        {
            try
            {
                //ayarlar!.SiteLogo = null;
                bool success = await _serviceManager.AyarlarService.SoftDeletePhotoAsync(id);
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                // Hata durumunda daha fazla bilgi al
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete("/Ayarlar/DeleteFile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile()
        {
            try
            {
                //ayarlar!.Program = null;
                bool success = await _serviceManager.AyarlarService.SoftDeleteFileAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata durumunda daha fazla bilgi al
                return Json(new { success = false, message = ex.Message });
            }
        }



        // POST: Ayarlar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SiteAdi,KurumAdi,SiteAltBaslik,Program")] Ayarlar ayarlar, IFormFile? Program, IFormFile? SiteLogo, IFormFile? SagLogo, IFormFile? SolLogo, IFormFile? Filigran, IFormFile? SiteArkaplani)
        {
            if (id != ayarlar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.AyarlarService.SoftUpdateAsync(ayarlar, Program, SiteLogo, SagLogo, SolLogo, Filigran, SiteArkaplani);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            Ayarlar model = await _serviceManager.AyarlarService.SoftGetAyarlarAsync();
            ayarlar.Filigran = model.Filigran;
            ayarlar.SiteArkaplani = model.SiteArkaplani;
            ayarlar.Sayfalar = model.Sayfalar;
            ayarlar.SagLogo = model.SagLogo;
            ayarlar.SiteLogo = model.SiteLogo;
            ayarlar.SolLogo = model.SolLogo;

            ViewBag.AcceptPhoto = _serviceManager.FileService.GetAcceptString(FileService.DosyaTuru.Photo);
            ViewBag.AcceptForm = _serviceManager.FileService.GetAcceptString(FileService.DosyaTuru.Belge);
            return View(ayarlar);
        }




        //////////////////////////////////////////////////////////////////////
        ////////////////////////////// SAYFALAR //////////////////////////////
        //////////////////////////////////////////////////////////////////////


        // GET: Ayarlar/EditSayfalar/5
        public async Task<IActionResult> EditSayfalar(int? id) // şimdilik hata vermesin diye parametreyi dokunmadım, sonradan sileceğim
        {
            if (id == null)
                return NotFound();
     
            return View(await _serviceManager.SayfalarService.SoftGetAllAsync());
        }


        // POST: Ayarlar/EditSayfalar/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSayfalar(IList<Sayfalar> model)
        {
            for (int sayfaIndex = 0; sayfaIndex < model.Count; sayfaIndex++)
            {
                var sayfa = model[sayfaIndex];

                // Veritabanından eski değeri çek ve takip edilmemesini sağla
                var eskiSayfa = await _serviceManager.SayfalarService.SoftFirstOrDefaultAsync(sayfa.Id);

                if (eskiSayfa != null && string.IsNullOrEmpty(sayfa.Url))
                {
                    // Eğer yeni modelde URL boşsa, eski URL'yi koru
                    sayfa.Url = eskiSayfa.Url;
                }

                ModelState.Remove($"Model[{sayfaIndex}].Url");

                if (sayfa.AltSayfalari != null)
                {
                    for (int altSayfaIndex = 0; altSayfaIndex < sayfa.AltSayfalari.Count; altSayfaIndex++)
                    {
                        var altSayfa = sayfa.AltSayfalari.ElementAt(altSayfaIndex);

                        // Veritabanından eski değeri çek ve takip edilmemesini sağla
                        var eskiAltSayfa = await _serviceManager.AltSayfaService.SoftFirstOrDefaultAsync(altSayfa.Id);

                        if (eskiAltSayfa != null && string.IsNullOrEmpty(altSayfa.Url))
                        {
                            // Eğer yeni modelde URL boşsa, eski URL'yi koru
                            altSayfa.Url = eskiAltSayfa.Url;
                        }

                        altSayfa.UstSayfa = sayfa;

                        ModelState.Remove($"Model[{sayfaIndex}].AltSayfalari[{altSayfaIndex}].UstSayfa");
                        ModelState.Remove($"Model[{sayfaIndex}].AltSayfalari[{altSayfaIndex}].Url");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.SayfalarService.SoftUpdateAsync(model);
                    SetTempMessage(result, "güncelleme");
                return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu => ({ex.Message})");
                }

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SayfaSifirla()
        {
            try
            {
                await _sayfaService.SayfalariSifirla();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
                throw;
            }

        }
        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Ayarlar bilgilerini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, ayarlar bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
