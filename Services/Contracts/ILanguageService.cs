using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace dafsem.Services.Contracts
{
    public interface ILanguageService
    {
        List<SelectListItem> GetSupportedLanguages();
    }
}
