using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dafsem.Models;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class ParaBirimiController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public ParaBirimiController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: ParaBirimi
        public async Task<IActionResult> Index()
        {
            IEnumerable<ParaBirimi> model = await _serviceManager.ParaBirimiService.SoftGetAllAsync();
            return View(model);
        }

        // GET: ParaBirimi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paraBirimi = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync((int)id);
            if (paraBirimi == null)
            {
                return NotFound();
            }

            return View(paraBirimi);
        }

        // GET: ParaBirimi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParaBirimi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kod,Ad,Sembol")] ParaBirimi paraBirimi)
        {
            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.ParaBirimiService.SoftAddAsync(paraBirimi);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }
            return View(paraBirimi);
        }

        // GET: ParaBirimi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var paraBirimi = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync((int)id);

            if (paraBirimi == null)
                return NotFound();

            return View(paraBirimi);
        }

        // POST: ParaBirimi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kod,Ad,Sembol")] ParaBirimi paraBirimi)
        {
            if (id != paraBirimi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.ParaBirimiService.SoftUpdateAsync(paraBirimi);
                    SetTempMessage(result, "güncelleme");
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty, "Güncelleme yaparken çakışma nedeniyle hata oluştu!");
                    return View(paraBirimi);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paraBirimi);
        }

        // GET: ParaBirimi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paraBirimi = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync((int)id);
            if (paraBirimi == null)
            {
                return NotFound();
            }

            return View(paraBirimi);
        }

        // POST: ParaBirimi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.ParaBirimiService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Para birimini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, para birimini {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
