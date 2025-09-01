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
    public class OdaTipleriController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public OdaTipleriController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: OdaTipleri
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.OdaTipleriService.SoftGetAllAsync());
        }

        // GET: OdaTipleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odaTipleri = await _serviceManager.OdaTipleriService.SoftFirstOrDefaultAsync((int)id);
            if (odaTipleri == null)
            {
                return NotFound();
            }

            return View(odaTipleri);
        }

        // GET: OdaTipleri/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();

            ViewBag.KonaklamaEvleri = await _serviceManager.KonaklamaService.SoftGeAllAsSelectListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms(int konaklamaId)
        {
            var odalar = await _serviceManager.OdaTipleriService.SoftGetRoomsByKonaklamaIdAsync(konaklamaId);

            if (!odalar.Any())
            {
                return PartialView("_NoRoomsFoundPartial");
            }

            return PartialView("_RoomListPartial", odalar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndGet([Bind("Id,OdaTipi,Ucret")] OdaTipleri odaTipleri, int Birim, int KonaklamaEvi)
        {
            odaTipleri.Birim = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync(Birim);
            ModelState[nameof(odaTipleri.Birim)]!.ValidationState = odaTipleri.Birim != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            odaTipleri.KonaklamaEvi = await _serviceManager.KonaklamaService.SoftFirstOrDefaultAsync(KonaklamaEvi);
            ModelState[nameof(odaTipleri.KonaklamaEvi)]!.ValidationState = odaTipleri.KonaklamaEvi != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.OdaTipleriService.SoftAddAsync(odaTipleri);
                if (result)
                {
                    SetTempMessage(result, "ekleme");
                    return await GetRooms(KonaklamaEvi);
                }
            }

            // Hata durumunda ModelState hatalarını döndür
            var errors = ModelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    ms => ms.Key, // Alan adı
                    ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray() // Hata mesajları
                );

            return BadRequest(new { Errors = errors });
        }
        // POST: OdaTipleri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OdaTipi,Ucret")] OdaTipleri odaTipleri, int Birim, int KonaklamaEvi)
        {
            odaTipleri.Birim = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync(Birim);
            ModelState[nameof(odaTipleri.Birim)]!.ValidationState = odaTipleri.Birim != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            odaTipleri.KonaklamaEvi = await _serviceManager.KonaklamaService.SoftFirstOrDefaultAsync(KonaklamaEvi);
            ModelState[nameof(odaTipleri.KonaklamaEvi)]!.ValidationState = odaTipleri.KonaklamaEvi != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.OdaTipleriService.SoftAddAsync(odaTipleri);
                SetTempMessage(result, "ekleme");

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            ViewBag.KonaklamaEvleri = await _serviceManager.KonaklamaService.SoftGeAllAsSelectListAsync();

            return View(odaTipleri);
        }

        // GET: OdaTipleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odaTipleri = await _serviceManager.OdaTipleriService.SoftFirstOrDefaultAsync((int)id);
            if (odaTipleri == null)
            {
                return NotFound();
            }

            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            ViewBag.KonaklamaEvleri = await _serviceManager.KonaklamaService.SoftGeAllAsSelectListAsync();

            return View(odaTipleri);
        }

        // POST: OdaTipleri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OdaTipi,Ucret")] OdaTipleri odaTipleri, int Birim, int KonaklamaEvi)
        {
            if (id != odaTipleri.Id)
            {
                return NotFound();
            }
            odaTipleri.Birim = await _serviceManager.ParaBirimiService.SoftFirstOrDefaultAsync(Birim);
            ModelState[nameof(odaTipleri.Birim)]!.ValidationState = odaTipleri.Birim != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            odaTipleri.KonaklamaEvi = await _serviceManager.KonaklamaService.SoftFirstOrDefaultAsync(KonaklamaEvi);
            ModelState[nameof(odaTipleri.KonaklamaEvi)]!.ValidationState = odaTipleri.KonaklamaEvi != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.OdaTipleriService.SoftUpdateAsync(odaTipleri);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception )
                {
                    ModelState.AddModelError(string.Empty, "Güncelleme sırasında hata oluştu");
                }
            }

            ViewBag.Birimler = await _serviceManager.ParaBirimiService.SoftGeAllAsSelectListAsync();
            ViewBag.KonaklamaEvleri = await _serviceManager.KonaklamaService.SoftGeAllAsSelectListAsync();

            return View(odaTipleri);
        }

        // GET: OdaTipleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var odaTipleri = await _serviceManager.OdaTipleriService.SoftFirstOrDefaultAsync((int)id);
            if (odaTipleri == null)
            {
                return NotFound();
            }

            return View(odaTipleri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOda(int id, int konaklamaId)
        {
            _ = await DeleteConfirmed(id);

            var odalar = await GetRooms(konaklamaId);
            return odalar;
        }

        // POST: OdaTipleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.OdaTipleriService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Oda bilgileri {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, oda bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
