using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class TarihlerController : Controller
    {
        private readonly IServiceManager _manager;

        public TarihlerController(IServiceManager manager)
        {
            _manager = manager;
        }

        //public string FormatDateWithMonthName(DateTime date)
        //{
        //    return date.ToString("dd-MMMM-yyyy", new CultureInfo("tr-TR")).ToUpper();
        //}

        // GET: AdminPanel/Tarihler
        public async Task<IActionResult> Index()
        {
            var tarihler = await _manager.TarihlerService.SoftGetAllAsync();
            return View(tarihler);
        }

        // GET: AdminPanel/Tarihler/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tarih = await _manager.TarihlerService.SoftFirstOrDefaultAsync(id);
            return tarih is null ? NotFound() : View(tarih);
        }

        // GET: AdminPanel/Tarihler/Create
        public IActionResult Create() => View();

        // POST: AdminPanel/Tarihler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarihler tarihler)
        {
            if (!ModelState.IsValid) return View(tarihler);

            var result = await _manager.TarihlerService.SoftAddAsync(tarihler);
            SetTempMessage(result, "ekleme");

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminPanel/Tarihler/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tarih = await _manager.TarihlerService.SoftFirstOrDefaultAsync(id);
            return tarih is null ? NotFound() : View(tarih);
        }

        // POST: AdminPanel/Tarihler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tarih,Aciklama")] Tarihler tarihler)
        {
            if (id != tarihler.Id) return BadRequest();

            if (!ModelState.IsValid) return View(tarihler);

            try
            {
                var result = await _manager.TarihlerService.SoftUpdateAsync(tarihler);
                SetTempMessage(result, "güncelleme");
            }
            catch (DbUpdateConcurrencyException)
            {
                HandleConcurrencyError(tarihler.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminPanel/Tarihler/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tarih = await _manager.TarihlerService.SoftFirstOrDefaultAsync(id);
            return tarih is null ? NotFound() : View(tarih);
        }

        // POST: AdminPanel/Tarihler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _manager.TarihlerService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");

            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Tarih {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, Tarih {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }

        private void HandleConcurrencyError(int id)
        {
            if (!_manager.TarihlerService.SoftTarihlerExists(id))
            {
                ModelState.AddModelError(string.Empty, "Bu kayıt artık mevcut değil");
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "Bu kayıt başka bir kullanıcı tarafından değiştirildi. Lütfen yeniden deneyin.");
            }
        }
        #endregion
    }
}
