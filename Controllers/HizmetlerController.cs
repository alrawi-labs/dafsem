using Microsoft.AspNetCore.Mvc;
using dafsem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class HizmetlerController : Controller
    {
        private readonly IServiceManager _serviceManager;
        public HizmetlerController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET: Hizmetler
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.HizmetlerService.SoftGetAllHizmetlerAsync();

            return View(model);
        }

        // GET: Hizmetler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmetler = await _serviceManager.HizmetlerService.SoftFirstOrDefaultAsync((int)id);
            if (hizmetler == null)
            {
                return NotFound();
            }

            return View(hizmetler);
        }

        // GET: Hizmetler/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.HizmetTuru = await _serviceManager.HizmetTuruService.SoftGeAllAsSelectListAsync();
            return View();
        }

        // POST: Hizmetler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hizmet,HizmetTuruId")] Hizmetler hizmetler)
        {
            if (hizmetler == null)
                return NotFound();

            await CheckHizmetTuru(hizmetler.HizmetTuruId, nameof(hizmetler.Turu));

            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.HizmetlerService.SoftAddAsync(hizmetler);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.HizmetTuru = await _serviceManager.HizmetTuruService.SoftGeAllAsSelectListAsync();
            return View(hizmetler);
        }

        // GET: Hizmetler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmetler = await _serviceManager.HizmetlerService.SoftFirstOrDefaultAsync((int)id);
            if (hizmetler == null)
            {
                return NotFound();
            }

            ViewBag.HizmetTuru = await _serviceManager.HizmetTuruService.SoftGeAllAsSelectListAsync();
            return View(hizmetler);
        }

        // POST: Hizmetler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hizmet,HizmetTuruId")] Hizmetler hizmetler)
        {
            if (id != hizmetler.Id)
            {
                return NotFound();
            }

            await CheckHizmetTuru(hizmetler.HizmetTuruId, nameof(hizmetler.Turu));

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.HizmetlerService.SoftUpdateAsync(hizmetler);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu => ({ex.Message})");
                }
            }

            ViewBag.HizmetTuru = await _serviceManager.HizmetTuruService.SoftGeAllAsSelectListAsync();
            return View(hizmetler);
        }

        // GET: Hizmetler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hizmetler = await _serviceManager.HizmetlerService.SoftFirstOrDefaultAsync((int)id);
            if (hizmetler == null)
            {
                return NotFound();
            }

            return View(hizmetler);
        }

        // POST: Hizmetler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.HizmetlerService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private async Task CheckHizmetTuru(int hizmetTuruId, string nameofModel)
        {
            var tmp = await _serviceManager.HizmetTuruService.SoftFirstOrDefaultAsync(hizmetTuruId)
               ?? throw new KeyNotFoundException($"Hizmet Türü (ID: {hizmetTuruId}) bulunamadı.");

            if (tmp != null)
                ModelState.Remove(nameofModel);
        }
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Hizmet bilgilerini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, hizmet bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
