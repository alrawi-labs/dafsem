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
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class HizmetTuruController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public HizmetTuruController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: HizmetTuru
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.HizmetTuruService.SoftGetAllAsync());
        }

        // GET: HizmetTuru/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmetTuru = await _serviceManager.HizmetTuruService.SoftFirstOrDefaultAsync((int)id);
            if (hizmetTuru == null)
            {
                return NotFound();
            }

            return View(hizmetTuru);
        }

        // GET: HizmetTuru/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HizmetTuru/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tur")] HizmetTuru hizmetTuru)
        {
            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.HizmetTuruService.SoftAddAsync(hizmetTuru);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }
            return View(hizmetTuru);
        }

        // GET: HizmetTuru/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmetTuru = await _serviceManager.HizmetTuruService.SoftFirstOrDefaultAsync((int)id);
            if (hizmetTuru == null)
            {
                return NotFound();
            }
            return View(hizmetTuru);
        }

        // POST: HizmetTuru/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tur")] HizmetTuru hizmetTuru)
        {
            if (id != hizmetTuru.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.HizmetTuruService.SoftUpdateAsync(hizmetTuru);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Güncelleme sırasında hata oluştu => (" + ex.Message + ")");
                }
            }
            return View(hizmetTuru);
        }

        // GET: HizmetTuru/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmetTuru = await _serviceManager.HizmetTuruService.SoftFirstOrDefaultAsync((int)id);
            if (hizmetTuru == null)
            {
                return NotFound();
            }

            return View(hizmetTuru);
        }

        // POST: HizmetTuru/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.HizmetTuruService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");

            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Hizmet türünü {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, hizmet türünü {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
