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
    public class KurulKategorileriController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public KurulKategorileriController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: KurulKategorileri
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.KurulKategorileriService.SoftGetAllAsync());
        }

        // GET: KurulKategorileri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurulKategorileri = await _serviceManager.KurulKategorileriService.SoftFirstOrDefaultAsync((int)id);

            if (kurulKategorileri == null)
            {
                return NotFound();
            }

            return View(kurulKategorileri);
        }

        // GET: KurulKategorileri/Create
        public async Task<IActionResult> Create()
        {

            // ViewBag ile view'e gönderiyoruz
            ViewBag.SiraList = await _serviceManager.KurulKategorileriService.SoftGetSiraAsSelectListAsync();


            //var altSayfalar = _context.AltSayfa.Select(n => new SelectListItem
            //{
            //    Value = n.Id.ToString(),
            //    Text = n.AltSayfaBaslik.ToString()
            //})
            //.ToList();

            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();


            return View();
        }

        // POST: KurulKategorileri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Baslik,Sira,SayfaId")] KurulKategorileri kurulKategorileri)
        {
            ModelState.Remove("Sayfa");
            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.KurulKategorileriService.SoftAddAsync(kurulKategorileri);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }

            // ViewBag ile view'e gönderiyoruz
            ViewBag.SiraList = await _serviceManager.KurulKategorileriService.SoftGetSiraAsSelectListAsync();
            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();

            return View(kurulKategorileri);
        }

        // GET: KurulKategorileri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurulKategorileri = await _serviceManager.KurulKategorileriService.SoftFirstOrDefaultAsync((int)id);
            if (kurulKategorileri == null)
                return NotFound();

            ViewBag.SiraList = await _serviceManager.KurulKategorileriService.SoftGetSiraAsSelectListAsync((int)id);
            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();

            return View(kurulKategorileri);
        }

        // POST: KurulKategorileri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Sira,SayfaId")] KurulKategorileri kurulKategorileri)
        {
            if (id != kurulKategorileri.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Sayfa");
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.KurulKategorileriService.SoftUpdateAsync(kurulKategorileri);
                    SetTempMessage(result, "güncelleme");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu => ({ex.Message})");
                }
            }

          
            ViewBag.SiraList = await _serviceManager.KurulKategorileriService.SoftGetSiraAsSelectListAsync();
            ViewBag.AltSayfaList = await _serviceManager.AltSayfaService.SoftGetAllAsSelectListAsync();
            return View(kurulKategorileri);
        }

        // GET: KurulKategorileri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var kurulKategorileri = await _serviceManager.KurulKategorileriService.SoftFirstOrDefaultAsync((int)id);
            if (kurulKategorileri == null)
                return NotFound();

            return View(kurulKategorileri);
        }

        // POST: KurulKategorileri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.KurulKategorileriService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Kurul Kategori bilgileri {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, kurul kategori bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
