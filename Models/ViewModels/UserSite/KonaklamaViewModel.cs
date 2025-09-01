using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace dafsem.Models.ViewModels.UserSite
{
    public class KonaklamaViewModel
    {
        [DisplayName("Konaklama Evi")]
        public required string KonaklamaEvi { get; set; }

        [DisplayName("Adres")]
        public required string Adres { get; set; }

        [DisplayName("Telefon")]

        public string? Tel { get; set; }


        [DisplayName("E-posta")]

        public string? Eposta { get; set; }


        [DisplayName("Web Sitesi")]

        public string? WebSitesi { get; set; }

        [DisplayName("Yıldız Sayısı")]
        public int? YildizSayisi { get; set; }

        [DisplayName("Kahvaltı Dahil Mi ?")]
        public string? KahvaltiDahilMi { get; set; }

        [DisplayName("Ücret Bilgisi:")]
        public IEnumerable<string>? Odalar { get; set; }
    }
}
