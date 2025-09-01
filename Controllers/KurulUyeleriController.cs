using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dafsem.Context;
using dafsem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using Azure;
using Microsoft.AspNetCore.Authorization;
using dafsem.Services.Contracts;
using dafsem.Models.ViewModels;
using dafsem.Services;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class KurulUyeleriController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public KurulUyeleriController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: KurulUyeleri
        public async Task<IActionResult> Index()
        {
            return View(await _serviceManager.KurulUyeleriService.SoftGetAllAsync());

        }

        // GET: KurulUyeleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var kurulUyeleri = await _serviceManager.KurulUyeleriService.SoftFirstOrDefaultAsync((int)id);
            if (kurulUyeleri == null)
                return NotFound();

            return View(kurulUyeleri);
        }

        // GET: KurulUyeleri/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Kategoriler = await _serviceManager.KurulKategorileriService.SoftGetAsSelectListAsync();
            ViewBag.Unvanlar = await _serviceManager.UnvanlarService.SoftGetAsSelectItemAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteToExcel(List<KurulUyeleri> kurulUyeleri)
        {
            if (kurulUyeleri == null || kurulUyeleri.Count == 0)
            {
                TempData["Error"] = "Veri bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                byte[] fileContent = await _serviceManager.KurulUyeleriService.SoftWriteToExcel(kurulUyeleri);
                string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "KurulUyeleriHataliKayitlar.xlsx";
                return File(fileContent, mimeType, fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Dosya oluşturulurken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF koruması
        public async Task<IActionResult> ReadExcelFile(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ErrorMessageModel model = new ErrorMessageModel()
                {
                    ErrorMessage = "Lütfen bir Excel dosyası seçin."
                };
                return PartialView("Components/Model/_NotFound", model);
            }

            // Dosya türü kontrolü
            if (excelFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                ErrorMessageModel model = new ErrorMessageModel()
                {
                    ErrorMessage = "Geçersiz dosya tipi. Lütfen bir Excel dosyası yükleyin."
                };
                return PartialView("Components/Model/_NotFound", model);
            }

            try
            {
                var result = await _serviceManager.KurulUyeleriService.SoftReadExcelFileAsync(excelFile);
                // result[0] = Doğru veriler, result[1] = Hatalı veriler
                return PartialView("Components/Model/_ExceldenUyeler", result);
            }
            catch (Exception ex)
            {
                ErrorMessageModel model = new ErrorMessageModel()
                {
                    ErrorMessage = "Bir hata oluştu: " + ex.Message
                };
                return PartialView("Components/Model/_NotFound", model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExceldenKurulUyeleriEkle([FromBody] List<KurulUyeImportDto> excelData)
        {
            try
            {
                var result = await _serviceManager.KurulUyeleriService.SoftExceldenKurulUyeleriEkle(excelData);
                if (result.success)
                {
                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Index", "KurulUyeleri")
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = result.message
                    });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapılabilir
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }

        // POST: KurulUyeleri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KurulUyeleri kurulUyeleri, int Kategori, int Unvan)
        {
            kurulUyeleri.KategoriId = await _serviceManager.KurulKategorileriService.SoftFirstOrDefaultAsync(Kategori);
            ModelState[nameof(kurulUyeleri.KategoriId)]!.ValidationState = kurulUyeleri.KategoriId != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            kurulUyeleri.Unvan = await _serviceManager.UnvanlarService.SoftFirstOrDefaultAsync(Unvan);
            ModelState[nameof(kurulUyeleri.Unvan)]!.ValidationState = kurulUyeleri.Unvan != null ? ModelValidationState.Valid : ModelValidationState.Invalid;


            if (ModelState.IsValid)
            {
                bool result = await _serviceManager.KurulUyeleriService.SoftAddAsync(kurulUyeleri);
                SetTempMessage(result, "ekleme");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = await _serviceManager.KurulKategorileriService.SoftGetAsSelectListAsync();
            ViewBag.Unvanlar = _serviceManager.UnvanlarService.SoftGetAsSelectItemAsync();

            return View(kurulUyeleri);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadExcelTemplate() // IFileService
        {
            try
            {
                // relativePath: wwwroot altındaki yolu belirtiyoruz
                var relativePath = Path.Combine("Uploads", "Site", "Docs", "Kurul-Uyeleri.xlsx");
                var fileName = "Kurul-Uyeleri.xlsx";
                var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                // ServiceManager üzerinden FileService'i çağırıyoruz
                var result = await _serviceManager.FileService.DownloadFileAsync(relativePath, fileName, mimeType);
                return File(result.fileBytes, result.mimeType, result.fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("Dosya bulunamadı.");
            }
            catch (Exception ex)
            {
                // Hata durumunda genel bir hata mesajı döndürebilirsiniz
                return StatusCode(500, "Sunucu hatası: " + ex.Message);
            }
        }


        // GET: KurulUyeleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var kurulUyeleri = await _serviceManager.KurulUyeleriService.SoftFirstOrDefaultAsync((int)id);
            if (kurulUyeleri == null)
                return NotFound();

            ViewBag.Kategoriler = await _serviceManager.KurulKategorileriService.SoftGetAsSelectListAsync();
            ViewBag.Unvanlar = _serviceManager.UnvanlarService.SoftGetAsSelectItemAsync();

            return View(kurulUyeleri);
        }

        // POST: KurulUyeleri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KurulUyeleri kurulUyeleri, int Kategori, int Unvan)
        {
            if (id != kurulUyeleri.Id)
            {
                return NotFound();
            }

            // Kategori atanıyor
            kurulUyeleri.KategoriId = await _serviceManager.KurulKategorileriService.SoftFirstOrDefaultAsync(Kategori);
            ModelState[nameof(kurulUyeleri.KategoriId)]!.ValidationState = kurulUyeleri.KategoriId != null ? ModelValidationState.Valid : ModelValidationState.Invalid;

            kurulUyeleri.Unvan = await _serviceManager.UnvanlarService.SoftFirstOrDefaultAsync(Unvan);
            ModelState[nameof(kurulUyeleri.Unvan)]!.ValidationState = kurulUyeleri.Unvan != null ? ModelValidationState.Valid : ModelValidationState.Invalid;


            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await _serviceManager.KurulUyeleriService.SoftUpdateAsync(kurulUyeleri);
                    SetTempMessage(result, "güncelleme");
                return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu =>({ex.Message})");
                }
                
            }

            ViewBag.Kategoriler = await _serviceManager.KurulKategorileriService.SoftGetAsSelectListAsync();
            ViewBag.Unvanlar = _serviceManager.UnvanlarService.SoftGetAsSelectItemAsync();

            return View(kurulUyeleri);
        }

        // GET: KurulUyeleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurulUyeleri = await _serviceManager.KurulUyeleriService.SoftFirstOrDefaultAsync((int)id);
            if (kurulUyeleri == null)
            {
                return NotFound();
            }

            return View(kurulUyeleri);
        }

        // POST: KurulUyeleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool result = await _serviceManager.KurulUyeleriService.SoftDeleteAsync(id);
            SetTempMessage(result, "silme");
            return RedirectToAction(nameof(Index));
        }

        #region Helpers
        private void SetTempMessage(bool success, string action)
        {
            var message = success
                ? $"Kurul Üye bilgileri {action} işlemi başarılı bir şekilde gerçekleştirildi"
                : $"Maalesef, kurul üye bilgileri {action} işlemi gerçekleştirilemedi. Lütfen tekrar deneyin.";

            TempData[success ? "Success" : "Error"] = message;
        }
        #endregion
    }
}
