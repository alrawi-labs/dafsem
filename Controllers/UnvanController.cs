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
    public class UnvanController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public UnvanController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Unvan
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.UnvanlarService.SoftGetAllAsync();
            return View(model);
        }

        // GET: Unvan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unvan = await _serviceManager.UnvanlarService.SoftFirstOrDefaultAsync((int)id);
            if (unvan == null)
            {
                return NotFound();
            }

            return View(unvan);
        }

        // GET: Unvan/Create
        public async Task<IActionResult> Create()
        {
            List<int> takenNumbers = await _serviceManager.UnvanlarService.SoftGetSira();

            var availableNumbers = Enumerable.Range(1, 999)
            .Where(n => !takenNumbers.Contains(n))
            .Select(n => new SelectListItem
            {
                Value = n.ToString(),
                Text = n.ToString()
            })
            .ToList();

            // ViewBag ile view'e gönderiyoruz
            ViewBag.SiraList = availableNumbers;
            return View();
        }

        // POST: Unvan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UnvanAdi,Sira")] Unvan unvan)
        {
            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.UnvanlarService.SoftAddAsync(unvan);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }
            return View(unvan);
        }


        // GET: Unvan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unvan = await _serviceManager.UnvanlarService.SoftFirstOrDefaultAsync((int)id);
            if (unvan == null)
            {
                return NotFound();
            }

            // Kullanılmış sıra numaralarını çekiyoruz
            List<int> takenNumbers = await _serviceManager.UnvanlarService.SoftGetSiraWithOut((int)id);


            // 1-999 arasındaki boş sıra numaralarını alıyoruz ve mevcut sıra numarasını ekliyoruz
            var availableNumbers = Enumerable.Range(1, 999)
                .Where(n => !takenNumbers.Contains(n) || n == unvan.Sira) // Mevcut sırayı dahil ediyoruz
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString(),
                    Selected = (n == unvan.Sira) // Seçili olanı belirtiyoruz
                })
                .ToList();

            ViewBag.SiraList = availableNumbers;
            return View(unvan);
        }

        // POST: Unvan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UnvanAdi,Sira")] Unvan unvan)
        {
            if (id != unvan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.UnvanlarService.SoftUpdateAsync(unvan);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ioe)
                {
                    ModelState.AddModelError("Sira", ioe.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                }
            }


            var takenNumbers = await _serviceManager.UnvanlarService.SoftGetSiraWithOut(id);

            var availableNumbers = Enumerable.Range(1, 999)
                .Where(n => !takenNumbers.Contains(n) || n == unvan.Sira)
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString(),
                    Selected = (n == unvan.Sira)
                })
                .ToList();

            ViewBag.SiraList = availableNumbers;

            return View(unvan);
        }

        // GET: Unvan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unvan = await _serviceManager.UnvanlarService.SoftFirstOrDefaultAsync((int)id);
            if (unvan == null)
            {
                return NotFound();
            }

            return View(unvan);
        }

        // POST: Unvan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.UnvanlarService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Ünvanı {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, ünvanı bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
