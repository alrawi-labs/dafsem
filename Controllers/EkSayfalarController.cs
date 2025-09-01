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
using dafsem.Models.ViewModels;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class EkSayfalarController : Controller
    {
        private readonly IServiceManager _serviceManager;
        public EkSayfalarController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: EkSayfalar
        public async Task<IActionResult> Index()
        {
            var ekSayfalar = await _serviceManager.EkSayfalarService.SoftGetAllSayfalarAsync();
            if (ekSayfalar == null)
                return NotFound();

            return View(ekSayfalar);
        }

        // GET: EkSayfalar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sayfa = await _serviceManager.EkSayfalarService.SoftFirstOrDefaultAsync((int)id);
            if (sayfa == null)
                return NotFound();

            // Bileşenleri ayrı çekiyoruz
            var bilesenler = await _serviceManager.EkSayfalarService.SoftGetAllBilesenlerAsync();

            var viewModel = new SayfaDetayViewModel
            {
                Sayfa = sayfa,
                Bilesenler = bilesenler
            };

            return View(viewModel);
        }

        // GET: EkSayfalar/Create
        public async Task<IActionResult> Create()
        {
            ViewData["BulunduguSayfaId"] = await _serviceManager.EkSayfalarService.SoftGetAllAsSelectList();
            ViewBag.Bilesenler = await _serviceManager.EkSayfalarService.SoftGetAllBilesenlerAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] EksayfaViewModel model)
        {
            if (model == null || model.Ekler == null || model.Ekler.Count == 0)
                return BadRequest(new { message = "Geçersiz veri gönderildi." });

            bool result = await _serviceManager.EkSayfalarService.SoftAddAsync(model);
            SetTempMessage(result, "ekleme");
            return ApiResponse(result, "ekleme");
        }

        // GET: EkSayfalar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            // Dropdown için verileri hazırla
            EkSayfalarEditViewModel viewModel = await _serviceManager.EkSayfalarService.GetEkSayfalarForEdit((int)id);

            ViewData["BulunduguSayfaId"] = await _serviceManager.EkSayfalarService.SoftGetAllAsSelectList(viewModel.BulunduguSayfaId);
            ViewBag.Bilesenler = await _serviceManager.EkSayfalarService.SoftGetAllBilesenlerAsync();
            return View(viewModel);
        }

        // POST: EkSayfalar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] EksayfaViewModel model)
        {
            if (id != model.Id)
                return BadRequest(new { message = "Geçersiz ID." });

            if (model == null)
                return BadRequest(new { message = "Geçersiz veri gönderildi." });

            try
            {
                bool result = await _serviceManager.EkSayfalarService.SoftEditAsync(model);
                SetTempMessage(result, "güncelleme");
                return ApiResponse(result, "güncelleme");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Bir hata oluştu: {ex.Message}" });
            }
        }



        // GET: EkSayfalar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sayfa = await _serviceManager.EkSayfalarService.SoftFirstOrDefaultAsync((int)id);

            if (sayfa == null)
            {
                return NotFound();
            }

            // Bileşenleri ayrı çekiyoruz
            var bilesenler = await _serviceManager.EkSayfalarService.SoftGetAllBilesenlerAsync();

            var viewModel = new SayfaDetayViewModel
            {
                Sayfa = sayfa,
                Bilesenler = bilesenler
            };

            return View(viewModel);
        }

        // POST: EkSayfalar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.EkSayfalarService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }


        #region Helpers
        private IActionResult ApiResponse(bool success, string action)
        {
            var message = success
                ? $"Sayfa {action} işlemi başarılı bir şekilde gerçekleştirildi."
                : $"Maalesef, sayfa {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            if (success)
                return Ok(new { success = true, message });

            return BadRequest(new { success = false, message });
        }
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Sayfa {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, sayfa {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }

        #endregion

    }
}
