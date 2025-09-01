using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dafsem.Models;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class BankaBilgileriController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public BankaBilgileriController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: BankaBilgileri
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.BankaBilgileriService.SoftGetAllAsync();
            return View(model);
        }

        // GET: BankaBilgileri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankaBilgileri = await _serviceManager.BankaBilgileriService.SoftFirstOrDefaultAsync((int)id);
            if (bankaBilgileri == null)
            {
                return NotFound();
            }

            return View(bankaBilgileri);
        }

        // GET: BankaBilgileri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankaBilgileri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BankaAdi,HesapSahibiAdi,IBAN")] BankaBilgileri bankaBilgileri)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.BankaBilgileriService.SoftAddAsync(bankaBilgileri);
                    if (result)
                    {
                        SetTempMessage(true, "ekleme");
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("IBAN", ex.Message);
                }
               
            }
            return View(bankaBilgileri);
        }

        // GET: BankaBilgileri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankaBilgileri = await _serviceManager.BankaBilgileriService.SoftFirstOrDefaultAsync((int)id);
            if (bankaBilgileri == null)
            {
                return NotFound();
            }
            return View(bankaBilgileri);
        }

        // POST: BankaBilgileri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BankaAdi,HesapSahibiAdi,IBAN")] BankaBilgileri bankaBilgileri)
        {
            if (id != bankaBilgileri.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.BankaBilgileriService.SoftUpdateAsync(bankaBilgileri);
                    if (result)
                    {
                        SetTempMessage(true, "güncelleme");
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }

            }
            SetTempMessage(false, "güncelleme");
            return View(bankaBilgileri);
        }

        // GET: BankaBilgileri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankaBilgileri = await _serviceManager.BankaBilgileriService.SoftFirstOrDefaultAsync((int)id);
            if (bankaBilgileri == null)
            {
                return NotFound();
            }

            return View(bankaBilgileri);
        }

        // POST: BankaBilgileri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _serviceManager.BankaBilgileriService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }


        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Banka bilgileri {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, banka bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
