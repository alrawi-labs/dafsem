using System.ComponentModel.DataAnnotations;

namespace dafsem.Models.ViewModels.Login
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Şifre en az 8 ve en fazla 40 karakter olmalıdır.")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Şifre doğrulama alanı boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifreyi Onayla")]
        public string ConfirmNewPassword { get; set; }
    }
}
