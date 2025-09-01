using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Controllers
{

    [Route("AdminPanel/[controller]/[action]")]
    public class AnaSayfaController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public AnaSayfaController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.AnaSayfaService.SoftGetAnaSayfaAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return View(await _serviceManager.AnaSayfaService.SoftGetAnaSayfaAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnaSayfa model, IFormFile? Afise)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.AnaSayfaService.SoftUpdateAsync(model, Afise);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu => ({ex.Message})");
                }
            }

            // Formda geçersiz veri varsa yeniden formu göster
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewSlider(IFormFile SliderPhoto)
        {
            Fotolar foto = await _serviceManager.AnaSayfaService.SoftAddSliderAsync(SliderPhoto);
            if (foto == null)
                return BadRequest("Dosya yüklenemedi.");

            return Json(new { success = true, id = foto.Id, yol = foto.Yol }); // Eklenen slider bilgilerini döndür
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            try
            {
                bool result = await _serviceManager.AnaSayfaService.SoftDeleteFotoAsync(id);

                if (result)
                    return Json(new { success = true, message = "Slider başarıyla silindi." });
                else
                    return NotFound(new { success = false, message = "Slider bulunamadı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Bir hata oluştu.", error = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAfise()
        {
            try
            {
                bool result = await _serviceManager.AnaSayfaService.SoftDeleteAfisAsync();
                return Json(new { success = result });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Ana sayfa bilgileri {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, ana sayfa bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
