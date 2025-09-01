using Microsoft.AspNetCore.Mvc.Rendering;

namespace dafsem.Models.ViewModels.UserSite
{
    public class HeaderViewModel
    {
        public string SiteAdi { get; set; }
        public string? DilKodu { get; set; }
        public IEnumerable<Sayfalar> Sayfalar { get; set; }
        public List<EkSayfalar>? EkSayfalar { get; set; }
        public List<SelectListItem>? Diller { get; set; }
    }
}
