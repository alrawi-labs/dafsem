using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dafsem.Context;
using dafsem.Models;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services;
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class KonaklamaController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public KonaklamaController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Konaklama
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.KonaklamaService.SoftGetAllAsync());
        }

        // GET: Konaklama/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konaklama = await _serviceManager.KonaklamaService.SoftFirstOrDefaultAsync((int)id);
            if (konaklama == null)
            {
                return NotFound();
            }

            return View(konaklama);
        }



        // GET: Konaklama/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Konaklama/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KonaklamaEvi,Adres,Tel,Eposta,WebSitesi,YildizSayisi,KahvaltiDahilMi")] Konaklama konaklama)
        {
            ModelState.Remove("Odalar");
            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.KonaklamaService.SoftAddAsync(konaklama);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Ekleme sırasında hata oluştu");
                    return View(konaklama);
                }
                ViewBag.SuccessMessage = "Konaklama başarıyla kaydedildi.";
                ViewBag.KonaklamaId = konaklama.Id;
                ViewBag.KonaklamaBaslik = konaklama.KonaklamaEvi;
                ViewBag.Birimler = _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
                return View(konaklama);
            }
            return View(konaklama);
        }

        // GET: Konaklama/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konaklama = await _serviceManager.KonaklamaService.SoftFirstOrDefaultAsync((int)id);
            if (konaklama == null)
            {
                return NotFound();
            }
            return View(konaklama);
        }

        // POST: Konaklama/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KonaklamaEvi,Adres,Tel,Eposta,WebSitesi,YildizSayisi,KahvaltiDahilMi")] Konaklama konaklama)
        {
            if (id != konaklama.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Odalar");
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.KonaklamaService.SoftUpdateAsync(konaklama);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(konaklama);
        }

        // GET: Konaklama/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konaklama = await _serviceManager.KonaklamaService.SoftFirstOrDefaultAsync((int)id);
            if (konaklama == null)
            {
                return NotFound();
            }

            return View(konaklama);
        }

        // POST: Konaklama/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.KonaklamaService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Konaklama bilgilerini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, konaklama bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
