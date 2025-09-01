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
    public class UcretlerController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public UcretlerController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Ucretler
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.UcretlerService.SoftGetAllAsync());
        }

        // GET: Ucretler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ucretler = await _serviceManager.UcretlerService.SoftFirstOrDefaultAsync((int)id);
            if (ucretler == null)
            {
                return NotFound();
            }

            return View(ucretler);
        }

        // GET: Ucretler/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            return View();
        }

        // POST: Ucretler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Baslik,Ucret")] Ucretler ucretler, int Birim)
        {
            ucretler.Birim = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync(Birim);

            ModelState[nameof(ucretler.Birim)]!.ValidationState = ucretler.Birim != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.UcretlerService.SoftAddAsync(ucretler);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            return View(ucretler);
        }

        // GET: Ucretler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ucretler = await _serviceManager.UcretlerService.SoftFirstOrDefaultAsync((int)id);
            if (ucretler == null)
            {
                return NotFound();
            }

            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            return View(ucretler);
        }

        // POST: Ucretler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Ucret")] Ucretler ucretler, int Birim)
        {
            if (id != ucretler.Id)
            {
                return NotFound();
            }

            ucretler.Birim = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync(Birim);
            ModelState[nameof(ucretler.Birim)]!.ValidationState = ucretler.Birim != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.UcretlerService.SoftUpdateAsync(ucretler);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            return View(ucretler);
        }

        // GET: Ucretler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ucretler = await _serviceManager.UcretlerService.SoftFirstOrDefaultAsync((int)id);

            if (ucretler == null)
            {
                return NotFound();
            }

            return View(ucretler);
        }

        // POST: Ucretler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.UcretlerService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Ücreti {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, ücret bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
