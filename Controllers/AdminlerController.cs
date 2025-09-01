using dafsem.Models;
using dafsem.Models.ViewModels;
using dafsem.Models.ViewModels.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Packaging;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dafsem.Controllers
{
    [Authorize]
    [Route("AdminPanel/[controller]/[action]")]
    public class AdminlerController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminlerController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users
                .Where(x => x.EmailConfirmed && (x.LockoutEnd == null || x.LockoutEnd <= DateTime.UtcNow)) // Engellenmemiş ve e-postası onaylanmış kullanıcılar
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

        // Kullanıcı bilgilerini gösterme
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var rol = await _userManager.GetRolesAsync(user);

            var userInfo = new UserViewModel
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Roles = string.Join(", ", rol)
            };

            return View(userInfo);
        }


        public async Task<IActionResult> UpdateProfile(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            var roleNames = await _userManager.GetRolesAsync(user);

            var userInfo = new UpdateProfileViewModel
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                CurrentPassword = null,
                NewPassword = null,
                ConfirmNewPassword = null,
                Roles = string.Join(", ", roleNames)

            };

            return View(userInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var currentUser = await _userManager.FindByIdAsync(model.Id.ToString());
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            // Kullanıcı bilgilerini güncelle
            currentUser.FullName = model.Name;
            currentUser.Email = model.Email;
            currentUser.UserName = model.Email;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError("NewPassword", "Yeni şifreler uyuşmuyor.");
                    return View(model);
                }

                var passwordChangeResult = await _userManager.ChangePasswordAsync(currentUser, model.CurrentPassword ?? "", model.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    ModelState.AddModelError("CurrentPassword", "Mevcut şifre hatalı veya şifre değiştirilemedi.");
                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(currentUser);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Profil bilgileri güncellenemedi.");
                return View(model);
            }

            return RedirectToAction("MyProfile");
        }

        [Authorize("RootRole")]
        public IActionResult AddNewUserByRoot()
        {
            var roles = _roleManager.Roles
                           .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                           .ToList();

            ViewData["Roles"] = roles;
            return View();
        }

        public IActionResult AddNewUser()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("RootRole")]
        public async Task<IActionResult> AddNewUserByRoot(RootAddNewUserViewModel model)
        {

            var roles = _roleManager.Roles
                     .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                     .ToList();


            if (ModelState.IsValid)
            {
                // Öncelikle seçilen rolün var olup olmadığını kontrol et
                if (!string.IsNullOrEmpty(model.Roles))
                {
                    var role = await _roleManager.FindByIdAsync(model.Roles);
                    if (role == null)
                    {
                        ViewData["Roles"] = roles;
                        ModelState.AddModelError("Roles", "Seçilen yetki geçersiz veya mevcut değil.");
                        return View(model); // Kullanıcı hiç eklenmeden hata döndür
                    }
                }


                // Kullanıcı oluşturma işlemi
                var user = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true // root olduğu için onaylanmış olarak girer hemen
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.Roles))
                    {
                        var role = await _roleManager.FindByIdAsync(model.Roles);
                        var roleResult = await _userManager.AddToRoleAsync(user, role.Name);

                        if (!roleResult.Succeeded)
                        {
                            ViewData["Roles"] = roles;
                            ModelState.AddModelError("Roles", "Seçilen yetki atanırken hata oluştu.");
                            return View(model); // Kullanıcı eklendiyse bile burada durdur
                        }
                    }

                    return RedirectToAction("Index", "Adminler");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            ViewData["Roles"] = roles;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewUser(AdminAddNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı oluşturma işlemi
                var user = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = false // eklenen admin olduğu için eklediği hesabın bir root hesabından onaylanması gerek
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync("AdminRole");
                    if (role == null)
                    {
                        ModelState.AddModelError("", "Hesap eklendi ancak yetki belirlemekte hata olmuştur");
                        return View(model);
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Hesap eklendi ancak yetki belirlemekte hata olmuştur");
                        return View(model);
                    }
                    return RedirectToAction("Index", "Adminler");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [Authorize("RootRole")]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            var roleNames = await _userManager.GetRolesAsync(user);
            var roleIds = await _roleManager.Roles
                            .Where(r => roleNames.Contains(r.Name))
                            .Select(r => r.Id)
                            .ToListAsync(); // Tüm rol ID'lerini liste olarak al


            var userInfo = new RootEditViewModel
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                NewPassword = null,
                ConfirmNewPassword = null,
                Roles = string.Join(", ", roleIds)

            };
            var roles = _roleManager.Roles
                         .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                         .ToList();
            ViewData["Roles"] = roles;
            return View(userInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("RootRole")]
        public async Task<IActionResult> Edit(RootEditViewModel model)
        {
            // Fetch roles for the dropdown
            var roles = _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                .ToList();
            ViewData["Roles"] = roles;

            // Validate model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate selected role
            if (!string.IsNullOrEmpty(model.Roles) && await _roleManager.FindByIdAsync(model.Roles) == null)
            {
                ModelState.AddModelError("Roles", "Seçilen yetki geçersiz veya mevcut değil.");
                return View(model);
            }

            // Fetch the current user
            var currentUser = await _userManager.FindByIdAsync(model.Id.ToString());
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            // Update user details
            currentUser.FullName = model.Name;
            currentUser.Email = model.Email;
            currentUser.UserName = model.Email;

            // Handle password update if new password is provided
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Yeni şifreler uyuşmuyor.");
                    return View(model);
                }

                var passwordUpdateResult = await UpdateUserPasswordWithoutCurrentPasswordAsync(currentUser, model.NewPassword);
                if (!passwordUpdateResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Şifre değiştirilemedi.");
                    return View(model);
                }
            }

            // Update user and roles
            var updateResult = await UpdateUserAndRolesAsync(currentUser, model.Roles);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı güncellenemedi.");
                return View(model);
            }

            // Redirect to index on success
            return RedirectToAction("Index");
        }

        // Helper method to update user password
        private async Task<IdentityResult> UpdateUserPasswordWithoutCurrentPasswordAsync(Users user, string newPassword)
        {
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return removePasswordResult;
            }

            return await _userManager.AddPasswordAsync(user, newPassword);
        }

        // Helper method to update user details and roles
        private async Task<IdentityResult> UpdateUserAndRolesAsync(Users user, string roleId)
        {
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return updateResult;
            }

            if (!string.IsNullOrEmpty(roleId))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        return removeRolesResult;
                    }
                }

                var role = await _roleManager.FindByIdAsync(roleId);
                if (role != null)
                {
                    return await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            return IdentityResult.Success;
        }

     

        [Authorize("RootRole")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = new UserViewModel
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Roles = string.Join(", ", roles)
            };
            return View(userInfo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize("RootRole")]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var currentUser = await _userManager.FindByIdAsync(Id);
            if (currentUser == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(currentUser);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı silinemedi.");
            }
            return RedirectToAction("Index");
        }


        [Authorize("RootRole")]
        public async Task<IActionResult> LockUser(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = new UserViewModel
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Roles = string.Join(", ", roles)
            };
            return View(userInfo);
        }

        [HttpPost, ActionName("LockUser")]
        [ValidateAntiForgeryToken]
        [Authorize("RootRole")]
        public async Task<IActionResult> LockUserConfirmed(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return RedirectToAction(nameof(Index));
            }


            user.LockoutEnd = DateTime.MaxValue;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Hesap kilitleme işlemi başarısız oldu");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index)); // İşlem başarılıysa listeye geri dön
        }

        [Authorize("RootRole")]
        public async Task<IActionResult> OnayBekleyenHesaplar()
        {
            var users = _userManager.Users.Where(x => !x.EmailConfirmed).ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("RootRole")]

        public async Task<IActionResult> OnayBekleyenHesaplar(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return RedirectToAction("OnayBekleyenHesaplar");
            }

            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Onay işlemi başarısız olmuştur");
                return RedirectToAction("OnayBekleyenHesaplar"); 
            }

            // Başarılı bir şekilde hesap aktif hale gelmiştir
            return RedirectToAction("OnayBekleyenHesaplar");
        }


        [Authorize("RootRole")]
        public async Task<IActionResult> EngellenenHesaplar()
        {
            var users = _userManager.Users
                .Where(x => x.LockoutEnd != null && x.LockoutEnd > DateTime.UtcNow) // Şu an kilitli olan hesapları getir
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("RootRole")]
        public async Task<IActionResult> EngellenenHesaplar(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return RedirectToAction("EngellenenHesaplar"); // Hata durumunda sayfayı yenile
            }

            user.LockoutEnd = null; // Engeli kaldır
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Engeli kaldırma işlemi başarısız olmuştur");
                return RedirectToAction("EngellenenHesaplar"); // Hata durumunda tekrar yönlendir
            }

            return RedirectToAction("EngellenenHesaplar"); // Başarıyla kaldırınca sayfayı yenile
        }


        

    }
}