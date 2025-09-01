using dafsem.Context;
using dafsem.Models.ViewModels;
using dafsem.Models.ViewModels.UserSite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Views.Shared.Components
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AplicationDbContext _context;

        public FooterViewComponent(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int dilId)
        {
            string dilKodu = await _context.Dil.Where(d => d.State && d.Id == dilId).Select(d => d.DilKodu).FirstOrDefaultAsync() ?? "tr";

            var mail = await _context.Iletisim.Where(i => i.State && i.DilId == dilId).Select(i => i.Eposta).FirstOrDefaultAsync();
            var siteAdi = await _context.Ayarlar.Where(a => a.State && a.DilId == dilId).Select(a => a.SiteAdi).FirstOrDefaultAsync();

            return View(new FooterViewModel { SiteAdi = siteAdi, EPosta = mail, DilKodu = dilKodu });
        }
    }
}
