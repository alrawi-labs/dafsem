using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dafsem.Context;
using dafsem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class KurallarController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public KurallarController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Kurallar
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.KurallarService.SoftGetAllAsync());
        }

        // GET: Kurallar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurallar = await _serviceManager.KurallarService.SoftFirstOrDefaultAsync((int)id);
            if (kurallar == null)
            {
                return NotFound();
            }

            return View(kurallar);
        }

        // GET: Kurallar/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.KuralTuru = await _serviceManager.KuralTuruService.SoftGeAllAsSelectListAsync();
            return View();
        }
   
        // POST: Kurallar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Metin,KuralTuruId")] Kurallar kurallar)
        {
            await CheckKuralTuru(kurallar.KuralTuruId,nameof(kurallar.Turu));

            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.KurallarService.SoftAddAsync(kurallar);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.KuralTuru = await _serviceManager.KuralTuruService.SoftGeAllAsSelectListAsync();
            return View(kurallar);
        }

        // GET: Kurallar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurallar = await _serviceManager.KurallarService.SoftFirstOrDefaultAsync((int)id);
            if (kurallar == null)
            {
                return NotFound();
            }

            ViewBag.KuralTuru = await _serviceManager.KuralTuruService.SoftGeAllAsSelectListAsync();
            return View(kurallar);
        }

        // POST: Kurallar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Metin,KuralTuruId")] Kurallar kurallar)
        {
            if (id != kurallar.Id)
            {
                return NotFound();
            }

            await CheckKuralTuru(kurallar.KuralTuruId, nameof(kurallar.Turu));

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.KurallarService.SoftUpdateAsync(kurallar);
                    SetTempMessage(result, "güncelleme");
                return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu => ({ex.Message})");
                }
            }

            ViewBag.KuralTuru = await _serviceManager.KuralTuruService.SoftGeAllAsSelectListAsync();
            return View(kurallar);
        }

        // GET: Kurallar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurallar = await _serviceManager.KurallarService.SoftFirstOrDefaultAsync((int)id);
            if (kurallar == null)
            {
                return NotFound();
            }

            return View(kurallar);
        }

        // POST: Kurallar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.KurallarService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");

            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private async Task CheckKuralTuru(int kuralTuruId, string nameofModel)
        {
            var tmp = await _serviceManager.KuralTuruService.SoftFirstOrDefaultAsync(kuralTuruId)
               ?? throw new KeyNotFoundException($"Kural Türü (ID: {kuralTuruId}) bulunamadı.");

            if (tmp != null)
                ModelState.Remove(nameofModel);
        }
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Kural bilgilerini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, kural bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
