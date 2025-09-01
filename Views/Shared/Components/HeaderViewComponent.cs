using dafsem.Context;
using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Models.ViewModels.UserSite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Views.Shared.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AplicationDbContext _context;

        public HeaderViewComponent(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int dilId)
        {
            string siteAdi = await _context.Ayarlar.Where(a => a.State && a.DilId == dilId).Select(a => a.SiteAdi).FirstOrDefaultAsync();

            var sayfalar = _context.Sayfalar.Where(s => s.State && s.DilId == dilId).Include(a => a.AltSayfalari.Where(a => a.State)).ToList();

            string dilKodu = await _context.Dil.Where(d => d.State && d.Id == dilId).Select(d => d.DilKodu).FirstOrDefaultAsync() ?? "tr";

            foreach (var sayfa in sayfalar)
            {
                try
                {
                    if (sayfa.AltSayfalari.Count == 0)
                    {
                        string[] urls = sayfa.Url.Split('/');
                        sayfa.Url = Url.Action(urls[1], urls[0]);
                    }
                }
                catch (Exception)
                {
                }

                foreach (var alt in sayfa.AltSayfalari)
                {
                    try
                    {
                        string[] urls = alt.Url.Split('/');
                        alt.Url = Url.Action(urls[1], urls[0]);
                    }
                    catch (Exception)
                    {
                    }
                }
            }


            var languages = await _context.Dil
                .AsNoTracking()
                .Where(d => d.State)
                .Select(d => new SelectListItem
                {
                    Value = d.DilKodu.ToLower(),
                    Text = d.DilAdi,
                    Selected = dilId != null && dilId == d.Id
                })
                .ToListAsync();

            var ekSayfalar = await _context.EkSayfalar
                .AsNoTracking()
                .Include(ek => ek.AltSayfalar)
                .Where(ek => ek.State && ek.DilId == dilId && ek.BulunduguSayfa == null)
                .Select(ek => new EkSayfalar
                {
                    SayfaBasligi = ek.SayfaBasligi,
                    AltSayfalar = ek.AltSayfalar,
                    Url = ek.Url
                })
                .ToListAsync();

            return View(new HeaderViewModel { SiteAdi = siteAdi, Sayfalar = sayfalar, DilKodu = dilKodu.ToLower(), Diller = languages, EkSayfalar = ekSayfalar });
        }
    }
}
