using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class DilController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public DilController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Dil
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.DilService.SoftGetAllDilAsync());
        }

        // GET: Dil/Create
        public IActionResult Create()
        {
            ViewBag.SupportedLanguages = _serviceManager.DilService.GetSupportedLanguages();
            return View();
        }

        public async Task<IActionResult> ChangeLanguage(string selectedLanguage)
        {
            if (!string.IsNullOrEmpty(selectedLanguage) && int.TryParse(selectedLanguage, out int id))
            {
                // Cookie oluştur ve 1 yıl sakla
                CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddYears(1) };
                Response.Cookies.Append("SelectedLanguage", selectedLanguage, option);

                // Veritabanından dil bilgisini al
                Dil? dil = await _serviceManager.DilService.SoftFirstOrDefaultAsync(id);

                if (dil != null)
                    TempData["Success"] = $"Seçilen dil {dil.DilAdi} olarak ayarlandı. Bundan sonra yapacağınız tüm işlemler {dil.DilAdi} olarak sisteme kaydedilecektir.";
                else
                    TempData["Error"] = "Seçilen dil bulunamadı!";
            }
            else
            {
                TempData["Error"] = "Geçersiz dil seçimi!";
            }

            // Eğer Referer yoksa ana sayfaya yönlendir
            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        }


        // POST: Dil/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DilKodu")] Dil dil)
        {
            // Onu otomatik olarak eklenecektir
            ModelState.Remove(nameof(dil.DilAdi));

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.DilService.SoftAddAsync(dil);
                    SetTempMessage(result, "ekleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException vex)
                {
                    ModelState.AddModelError("DilKodu", vex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Ekleme sırasında hata oluştu => ({ex.Message})");
                }

            }
            ViewBag.SupportedLanguages = _serviceManager.DilService.GetSupportedLanguages();
            return View(dil);
        }


        // GET: Dil/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dil = await _serviceManager.DilService.SoftFirstOrDefaultAsync((int)id);
            if (dil == null)
                return NotFound();

            return View(dil);
        }

        // POST: Dil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.DilService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Dil {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, dil {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion

    }
}
