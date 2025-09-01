using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dafsem.Context;
using dafsem.Models;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;
using dafsem.Models.ViewModels;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class IletisimController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public IletisimController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Iletisim
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.IletisimService.SoftGetLastAsync());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> yeniTelefon([FromBody] TelefonDto request)
        {
            try
            {
                await _serviceManager.IletisimService.SoftTelefonAddAsync(request);
                var telefonlar = await _serviceManager.IletisimService.SoftGetTelefonlar();
                return Json(new { success = true, message = "Telefonlar başarıyla kaydedildi.", telefonlar = telefonlar });
            }
            catch (Exception ex)
            {
                var telefonlar = await _serviceManager.IletisimService.SoftGetTelefonlar();
                return Json(new { success = false, message = ex.Message, telefonlar = telefonlar });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetTelefon(int id)
        {
            var telefon = await _serviceManager.TelefonlarService.SoftFirstOrDefault(id);
            if (telefon == null)
                return Json(new { success = false, message = "Telefon bulunamadı." });

            return Json(new { success = true, telefon });
        }


        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTelefon(int id, [FromBody] TelefonDto request)
        {
            try
            {
                await _serviceManager.TelefonlarService.SoftUpdateAsync(id, request);
                return Json(new { success = true, message = "Telefon başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTelefon(int id)
        {
            try
            {
                await _serviceManager.TelefonlarService.SoftDeleteAsync(id);
                return Ok(new { success = true, message = "Telefon numarası başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        // GET: Iletisim/Edit/5
        public async Task<IActionResult> Edit(int? id) // bu id parametresine gerek yok ama hata vermesin diye şimdilik silmiyorum
        {
            var iletisim = await _serviceManager.IletisimService.SoftGetLastAsync();
            return View(iletisim);
        }

        // POST: Iletisim/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Eposta,Adres")] Iletisim iletisim)
        {
            if (id != iletisim.Id) // buna gerek yoktur ama şimdilik hata vermesin diye silmiyorum.
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.IletisimService.SoftUpdateAsync(iletisim);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu => ({ex.Message})");
                }
            }
            return View(iletisim);
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"İletişim bilgilerini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, iletişim bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
