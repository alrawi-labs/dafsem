using System.ComponentModel.DataAnnotations;

namespace dafsem.Models.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Key]
        public string Id { get; set; } = null!; // required yerine null! 

        [Required(ErrorMessage = "İsim alanı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "İsim en fazla 50 karakter uzunluğunda olabilir.")]
        [Display(Name = "Admin Adı")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = null!;

        [Display(Name = "Yetki")]
        public string? Roles { get; set; }

        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Mevcut şifre en az 8 ve en fazla 40 karakter olmalıdır.")]
        [Display(Name = "Mevcut Şifre")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Şifre en az 8 ve en fazla 40 karakter olmalıdır.")]
        [Display(Name = "Yeni Şifre")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifreyi Onayla")]
        public string? ConfirmNewPassword { get; set; }
    }
}
