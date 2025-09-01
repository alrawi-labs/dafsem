using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace dafsem.Models.ViewModels.UserSite
{
    public class IletisimViewModel
    {
        [DisplayName("Eposta")]
        public string? Eposta { get; set; }

        [DisplayName("Adres")]
        public string? Adres { get; set; }

        [DisplayName("Telefon")]
        public ICollection<Telefonlar>? Telefonlar { get; set; }
    }
}
