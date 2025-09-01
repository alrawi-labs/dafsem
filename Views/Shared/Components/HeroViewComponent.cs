using dafsem.Context;
using dafsem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Views.Shared.Components
{
    public class HeroViewComponent : ViewComponent
    {
        private readonly AplicationDbContext _context;

        public HeroViewComponent(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int dilId)
        {
            string dilKodu = await _context.Dil.Where(d => d.State && d.Id == dilId).Select(d => d.DilKodu).FirstOrDefaultAsync() ?? "tr";

            var ayarlar = await _context.Ayarlar
                .Where(a => a.State && a.DilId == dilId)
             .Select(x => new HeroViewModel
             {
                 SiteArkaplaniYol = x.SiteArkaplani != null ? x.SiteArkaplani.Yol : null,
                 SagLogoYol = x.SagLogo != null ? x.SagLogo.Yol : null,
                 SolLogoYol = x.SolLogo != null ? x.SolLogo.Yol : null,
                 SiteAltBaslik = x.SiteAltBaslik,
                 KurumAdi = x.KurumAdi,
                 DilKodu = dilKodu
             }).FirstOrDefaultAsync();


            return View(ayarlar);
        }

    }
}
