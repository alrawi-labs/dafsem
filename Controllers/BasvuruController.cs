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
using dafsem.Services;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class BasvuruController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public BasvuruController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: Basvuru
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.BasvuruService.SoftGetLastAsync());
        }

        // GET: Basvuru/Edit/5
        public async Task<IActionResult> Edit()
        {

            ViewBag.AcceptForm = _serviceManager.FileService.GetAcceptString(FileService.DosyaTuru.Doc);
            return View(await _serviceManager.BasvuruService.SoftGetLastAsync());
        }

        // POST: Basvuru/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,UstMetin,Form,AltMetin")] Basvuru basvuru, IFormFile? BasvuruForm)
        {
            if (!ModelState.IsValid)
                return View(basvuru);
            try
            {
                var result = await _serviceManager.BasvuruService.SoftUpdateAsync(basvuru, BasvuruForm);
                if (!result)
                    return NotFound();

                SetTempMessage(result, "güncelleme");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.AcceptForm = _serviceManager.FileService.GetAcceptString(FileService.DosyaTuru.Doc);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(basvuru);
            }

        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForm()
        {
            bool result = await _serviceManager.BasvuruService.SoftDeleteForm();
            var message = result
                ? $"Başvuru forumunu silme işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, başvuru formunu silme işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[result ? "Success" : "Error"] = message;
            return Ok();
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Başvuru bilgilerini {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, başvuru bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion


    }
}
