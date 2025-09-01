using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace dafsem.Services
{
    public class LanguageService : ILanguageService
    {




        /// <summary>
        /// SupportedLanguage enum'undaki değerleri, SelectListItem koleksiyonuna dönüştürür.
        /// </summary>
        public List<SelectListItem> GetSupportedLanguages()
        {
            return Enum.GetValues(typeof(SupportedLanguage))
                       .Cast<SupportedLanguage>()
                       .Select(lang => new SelectListItem
                       {
                           // Kullanıcıya gösterilecek metin Display attribute'dan alınır.
                           Text = GetDisplayName(lang),
                           // Değer olarak enum'un string karşılığı kullanılabilir.
                           Value = lang.ToString()
                       })
                       .ToList();
        }
        /// <summary>
        /// Enum üyesinin DisplayAttribute'ünden tanımlı adı döner.
        /// </summary>
        private string GetDisplayName(SupportedLanguage language)
        {
            FieldInfo fieldInfo = language.GetType().GetField(language.ToString());
            DisplayAttribute displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute != null ? displayAttribute.Name : language.ToString();
        }

        public enum SupportedLanguage
        {
            [Display(Name = "Türkçe")]
            TR,

            [Display(Name = "İngilizce")]
            EN,

            [Display(Name = "Arapça")]
            AR,

            [Display(Name = "Fransızca")]
            FR,

            [Display(Name = "Almanca")]
            DE,

            [Display(Name = "İspanyolca")]
            ES,

            [Display(Name = "İtalyanca")]
            IT,

            [Display(Name = "Portekizce")]
            PT,

            [Display(Name = "Rusça")]
            RU,

            [Display(Name = "Çince (Basitleştirilmiş)")]
            ZH,

            [Display(Name = "Japonca")]
            JA,

            [Display(Name = "Korece")]
            KO
        }
    }
}
