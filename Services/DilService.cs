using Azure;
using dafsem.Components;
using dafsem.Context;
using dafsem.Models;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace dafsem.Services
{
    public class DilService : IDilService
    {
        private readonly AplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DilService(AplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ICollection<Dil>> SoftGetAllDilAsync()
        {
            await TurkceKontrolu();
            return await _context.Dil
                .AsNoTracking()
                .Where(d => d.State)
                .ToListAsync();
        }
        public async Task<List<SelectListItem>> SoftGetAllDilAsSelectListAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            await TurkceKontrolu();

            // Cookie'den seçili dili al
            string? selectedLanguageId = httpContext.Request.Cookies["SelectedLanguage"];

            var diller = await _context.Dil
                .AsNoTracking()
                .Where(d => d.State)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.DilAdi,
                    Selected = selectedLanguageId != null && selectedLanguageId == d.Id.ToString()
                })
                .ToListAsync();

            return diller;
        }

        private async Task<Dil> SoftGetDefaultDilAsync()
        {
            Dil dil = await _context.Dil.FirstOrDefaultAsync(d => d.DilKodu == "TR");
            if (dil == null)
                await TurkceKontrolu();

            dil = await _context.Dil.FirstOrDefaultAsync(d => d.DilKodu == "TR");
            return dil;
        }

        public async Task<string> GetKodOfDilByID(int id)
        {
            return await _context.Dil
                .AsNoTracking()
                .Where(d => d.Id == id && d.State)
                .Select(d => d.DilKodu)
                .FirstOrDefaultAsync() ?? "";
        }

        public async Task<int> SoftGetDilIdFromCookie()
        {
            int id = 0;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                string? selectedLanguageId = httpContext.Request.Cookies["SelectedLanguage"];

                if (!int.TryParse(selectedLanguageId, out id))
                {
                    httpContext.Response.Cookies.Delete("SelectedLanguage");
                    Dil tmp = await SoftGetDefaultDilAsync();
                    return tmp.Id;
                }
            }



            Dil? dil = await SoftFirstOrDefaultAsync(id);
            if (dil == null)
            {
                if (httpContext != null)
                    httpContext.Response.Cookies.Delete("SelectedLanguage");

                Dil tmp = await SoftGetDefaultDilAsync();
                return tmp.Id;
            }


            return id;
        }



        public async Task<Dil?> SoftFirstOrDefaultAsync(int id)
        {
            return await _context.Dil
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.State);
        }
        public async Task<bool> SoftAddAsync(Dil dil)
        {
            if (dil == null)
                return false;

            try
            {
                // Geçersiz bir dil kodu eklenmesini engelle
                string? dilAdi = GetLanguageName(dil.DilKodu);
                if (dilAdi == null)
                    throw new ValidationException("Geçersiz dil seçildi. Lütfen geçerli bir dil seçtiğinizden emin olun.");

                // State = true olan aynı DilKodu'na sahip başka bir kayıt var mı?
                bool exists = await _context.Dil
                    .AnyAsync(d => d.DilKodu == dil.DilKodu && d.State);

                if (exists)
                    throw new ValidationException($"'{dilAdi}' dili sistemde zaten mevcut.");

                // Yeni dili ekle
                dil.State = true;
                dil.DilAdi = dilAdi;
                await _context.Dil.AddAsync(dil);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        private async Task<Dil?> SoftFindAsync(int id)
        {
            var dil = await _context.Dil.FindAsync(id);
            return (dil != null && dil.State) ? dil : null;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            Dil? dil = await SoftFindAsync(id);
            if (dil == null)
                return false;

            string? dilAdi = GetLanguageName(dil.DilKodu);
            if (dilAdi == null)
                return false;

            dil.State = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> TurkceKontrolu()
        {
            var turkce = await _context.Dil.FirstOrDefaultAsync(d => d.DilKodu == "TR");

            if (turkce == null)
            {
                await _context.Dil.AddAsync(new Dil()
                {
                    DilAdi = "Türkçe",
                    DilKodu = "TR",
                    State = true
                });

                await _context.SaveChangesAsync();

                return true;
            }

            if (!turkce.State)
            {
                turkce.State = true;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public List<SelectListItem> GetSupportedLanguages()
        {
            return Enum.GetValues(typeof(SupportedLanguage))
                       .Cast<SupportedLanguage>()
                       .Select(lang => new SelectListItem
                       {
                           Text = GetDisplayName(lang),
                           Value = lang.ToString()
                       })
                       .ToList();
        }

        private string? GetLanguageName(string dilKodu)
        {
            if (Enum.TryParse(typeof(SupportedLanguage), dilKodu, true, out var language))
            {
                return GetDisplayName((SupportedLanguage)language);
            }
            return null;
        }

        private string GetDisplayName(SupportedLanguage language)
        {
            FieldInfo fieldInfo = language.GetType().GetField(language.ToString());
            DisplayAttribute displayAttribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? language.ToString();
        }
    }
}
