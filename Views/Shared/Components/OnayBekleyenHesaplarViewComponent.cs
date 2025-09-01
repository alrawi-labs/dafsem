using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using dafsem.Models.ViewModels;
using dafsem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace dafsem.ViewComponents
{
    public class OnayBekleyenHesaplarViewComponent : ViewComponent
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<Users> _userManager;

        public OnayBekleyenHesaplarViewComponent(IAuthorizationService authorizationService, UserManager<Users> userManager)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, "RootRole");

            if (!authorizationResult.Succeeded)
            {
                return Content(""); // Eğer yetki yoksa boş döndür.
            }

            var users = _userManager.Users
                .Where(u => !u.EmailConfirmed)
                .AsNoTracking()
                .ToList();

            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Kullanıcının rollerini al
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Email = user.Email,
                    Roles = string.Join(", ", roles) // Rolleri string olarak birleştir
                });
            }

            return View(userList);
        }
    }
}
