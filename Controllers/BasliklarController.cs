using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dafsem.Models;
using dafsem.Services;
using Microsoft.EntityFrameworkCore;
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class BasliklarController : Controller
    {
        private readonly IServiceManager _manager;

        public BasliklarController(IServiceManager manager)
        {
            _manager = manager;
        }

        // GET: AdminPanel/Basliklar
        public async Task<IActionResult> Index()
        {
            var basliklar = await _manager.BaslikService.SoftGetAllAsync();
            return View(basliklar);
        }

        // GET: AdminPanel/Basliklar/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var baslik = await _manager.BaslikService.SoftFirstOrDefaultAsync(id);
            return baslik is null ? NotFound() : View(baslik);
        }

        // GET: AdminPanel/Basliklar/Create
        public IActionResult Create() => View();

        // POST: AdminPanel/Basliklar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Basliklar basliklar)
        {
            if (!ModelState.IsValid) return View(basliklar);

            var result = await _manager.BaslikService.SoftAddAsync(basliklar);
            SetTempMessage(result, "ekleme");

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminPanel/Basliklar/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var baslik = await _manager.BaslikService.SoftFirstOrDefaultAsync(id);
            return baslik is null ? NotFound() : View(baslik);
        }

        // POST: AdminPanel/Basliklar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik")] Basliklar basliklar)
        {
            if (id != basliklar.Id) return BadRequest();

            if (!ModelState.IsValid) return View(basliklar);

            try
            {
                var result = await _manager.BaslikService.SoftUpdateAsync(basliklar);
                SetTempMessage(result, "güncelleme");
            }
            catch (Exception)
            {
                HandleConcurrencyError(basliklar.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminPanel/Basliklar/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var baslik = await _manager.BaslikService.SoftFirstOrDefaultAsync(id);
            return baslik is null ? NotFound() : View(baslik);
        }

        // POST: AdminPanel/Basliklar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _manager.BaslikService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");

            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Başlık {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, Başlık {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }

        private void HandleConcurrencyError(int id)
        {
            if (!_manager.BaslikService.SoftBasliklarExists(id))
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