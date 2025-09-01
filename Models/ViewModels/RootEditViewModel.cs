using System.ComponentModel.DataAnnotations;

namespace dafsem.Models.ViewModels
{
    public class RootEditViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Ad boş bırakılamaz.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad en az 2, en fazla 50 karakter olmalıdır.")]
        [Display(Name = "Admin Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [StringLength(40, MinimumLength = 8, ErrorMessage = "Şifre en az 8 ve en fazla 40 karakter olmalıdır.")]
        [Display(Name = "Yeni Şifre")]
        public string? NewPassword { get; set; } // Nullable yaparak zorunlu olmaktan çıkarıyoruz.

        [Display(Name = "Yeni Şifreyi Onayla")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? ConfirmNewPassword { get; set; } // Nullable yaparak zorunlu olmaktan çıkarıyoruz.

        [Required(ErrorMessage = "Yetki boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Yetki en fazla 50 karakter uzunluğunda olabilir.")]
        [Display(Name = "Yetki")]
        public required string Roles { get; set; }
    }
}