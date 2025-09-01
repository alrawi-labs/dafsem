using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace dafsem.Models.ViewModels.UserSite
{
    public class BasvuruViewModel
    {

        [DisplayName("Başvuru Sayfasında Üst Metin")]
        public string? UstMetin { get; set; }

        [DisplayName("Başvuru Formu")]
        public string? Form { get; set; }

        [DisplayName("Başvuru Sayfasında Alt Metin")]
        public string? AltMetin { get; set; }
        [DisplayName("E-Posta")]
        public string? EPosta { get; set; }
    }
}
