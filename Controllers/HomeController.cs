using dafsem.Models.ViewModels;
using dafsem.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dafsem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public IActionResult ChangeLanguage(string selectedLanguage, string actionName)
        {
            // Eðer action boþ deðilse ve geçerli bir action'a yönlendirme yapýlabiliyorsa oraya git
            if (!string.IsNullOrEmpty(actionName))
                return RedirectToAction(actionName, new { dilKodu = selectedLanguage });

            // Eðer action hatalý veya boþsa Index'e yönlendir
            return RedirectToAction(nameof(Index), new { dilKodu = selectedLanguage });
        }


        [Route("{dilKodu?}")]
        [Route("anasayfa/{dilKodu?}")]
        public async Task<IActionResult> Index(string dilKodu = "tr") // Varsayýlan 'tr' olsun
        {
            
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var anasayfa = await _homeService.GetAnaSayfa(dilId);
            if (anasayfa == null)
                return NotFound();
            ViewBag.tarihler = await _homeService.GetTarihler(dilId);

            ViewData["title"] = await _homeService.GetTitleOfSayfa("Home/Index", dilId);

            return View(anasayfa);
        }


        [Route("duzenlemekurulu/{dilKodu?}")]
        public async Task<IActionResult> DuzenlemeKurulu(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetKategoriUyeler("Home/duzenlemekurulu", dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/duzenlemekurulu", dilId);

            return View("KategoriUyeleriView", model);
        }

        [Route("bilimkurulu/{dilKodu?}")]
        public async Task<IActionResult> BilimKurulu(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetKategoriUyeler("home/bilimkurulu", dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/bilimkurulu", dilId);

            return View("KategoriUyeleriView", model);
        }

        [Route("davetlikonusmacilar/{dilKodu?}")]
        public async Task<IActionResult> DavetliKonusmacilar(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetKategoriUyeler("home/davetlikonusmacilar", dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/davetlikonusmacilar", dilId);

            return View("KategoriUyeleriView", model);
        }

        [Route("basliklar/{dilKodu?}")]
        public async Task<IActionResult> Basliklar(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetBasliklar(dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/basliklar", dilId);


            return View("KategoriUyeleriView", model); // Model artýk liste olarak dönüyor
        }

        [Route("program/{dilKodu?}")]
        public async Task<IActionResult> Program(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var program = await _homeService.GetProgram(dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/Program", dilId);

            return View("program", program);
        }

        [Route("tarihler/{dilKodu?}")]
        public async Task<IActionResult> Tarihler(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetTarihler(dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/Tarihler", dilId);

            return View(model);
        }

        [Route("yazimkurallari/{dilKodu?}")]
        public async Task<IActionResult> YazimKurallari(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            string[] kurallar = await _homeService.GetKurallar("Home/yazimkurallari", dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/yazimkurallari", dilId);

            return View("KurallarView", kurallar);
        }

        [Route("sunumkurallari/{dilKodu?}")]
        public async Task<IActionResult> SunumKurallari(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            string[] kurallar = await _homeService.GetKurallar("Home/sunumkurallari", dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/sunumkurallari", dilId);

            return View("KurallarView", kurallar);
        }

        [Route("ucretler/{dilKodu?}")]
        public async Task<IActionResult> Ucretler(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetUcretHizmetBanka(dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/Ucretler", dilId);

            return View(model);
        }

        [Route("konaklama/{dilKodu?}")]
        public async Task<IActionResult> Konaklama(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetKonaklama(dilId);
            ViewData["title"] = await _homeService.GetTitleOfAltSayfa("Home/konaklama", dilId);

            return View(model);
        }

        [Route("basvuru/{dilKodu?}")]
        public async Task<IActionResult> Basvuru(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetBasvuru(dilId);
            if (model == null)
                return NotFound();

            ViewData["title"] = await _homeService.GetTitleOfSayfa("Home/Basvuru", dilId);

            return View(model);
        }

        [Route("iletisim/{dilKodu?}")]
        public async Task<IActionResult> Iletisim(string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;

            var model = await _homeService.GetIletisim(dilId);
            ViewData["title"] = await _homeService.GetTitleOfSayfa("Home/Iletisim", dilId);

            return View(model);
        }

        [Route("sayfa/{url?}/{dilKodu?}")]
        public async Task<IActionResult> EkSayfa(string url, string dilKodu = "tr")
        {
            int dilId = await _homeService.GetIdbyDilKodu(dilKodu);
            ViewData["dilId"] = dilId;


            SayfaDetayViewModel viewModel = await _homeService.GetEkSayfa(url);
            ViewData["title"] = viewModel.Sayfa.SayfaBasligi;

            return View(viewModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code)
        {
            var errorViewModel = new ErrorViewModel
            {
                ErrorCode = code ?? 0 // Eðer kod yoksa varsayýlan olarak 0 ata
            };

            return View(errorViewModel);
        }

    }
}
