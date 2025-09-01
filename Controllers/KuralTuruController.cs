using Microsoft.AspNetCore.Mvc;
using dafsem.Models;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class KuralTuruController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public KuralTuruController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: KuralTuru
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.KuralTuruService.SoftGetAllAsync());
        }

        // GET: KuralTuru/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kuralTuru = await _serviceManager.KuralTuruService.SoftFirstOrDefaultAsync((int)id);
            if (kuralTuru == null)
            {
                return NotFound();
            }

            return View(kuralTuru);
        }

        // GET: KuralTuru/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();
            return View();
        }

        // POST: KuralTuru/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tur,SayfaId")] KuralTuru kuralTuru)
        {
            ModelState.Remove("Sayfa");
            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.KuralTuruService.SoftAddAsync(kuralTuru);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();
            return View(kuralTuru);
        }

        // GET: KuralTuru/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kuralTuru = await _serviceManager.KuralTuruService.SoftFirstOrDefaultAsync((int)id);
            if (kuralTuru == null)
            {
                return NotFound();
            }

            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();
            return View(kuralTuru);
        }

        // POST: KuralTuru/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tur, SayfaId")] KuralTuru kuralTuru)
        {
            if (id != kuralTuru.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Sayfa");
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.KuralTuruService.SoftUpdateAsync(kuralTuru);
                    SetTempMessage(result, "güncelleme");
                return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu. => ({ex.Message})");
                }
            }

            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();
            return View(kuralTuru);
        }

        // GET: KuralTuru/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kuralTuru = await _serviceManager.KuralTuruService.SoftFirstOrDefaultAsync((int)id);
            if (kuralTuru == null)
            {
                return NotFound();
            }

            return View(kuralTuru);
        }

        // POST: KuralTuru/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.KuralTuruService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Kural türünü {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, kural türü bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
