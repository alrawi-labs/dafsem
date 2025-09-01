using dafsem.Models;
using dafsem.Models.ViewModels.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dafsem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [Route("Giris")]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity!.IsAuthenticated) // Eğer kullanıcı giriş yapmışsa
            {
                return RedirectToAction("Index", "AnaSayfa"); // Direkt Index sayfasına yönlendir
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(); // Kullanıcı giriş yapmamışsa login sayfasını göster
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Giris")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                // Hesap doğrulama ve kilitlenme kontrolü
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("Email", "Hesabınız doğrulanmamış.");
                    return View(model);
                }

                if (user != null && await userManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError("Email", "Hesabınız kilitlenmiştir. Lütfen daha sonra tekrar deneyin veya yetkililerle iletişime geçin.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "AnaSayfa");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Email", "Hesabınız kilitlenmiştir. Lütfen daha sonra tekrar deneyin veya yetkililerle iletişime geçin.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Password", "Email veya şifre hatalı.");
                    return View(model);
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcıyı çıkış yaptır
            await signInManager.SignOutAsync();

            // Çerezleri temizle
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Giriş sayfasına yönlendir
            return RedirectToAction("Login", "Account");
        }


    }
}
