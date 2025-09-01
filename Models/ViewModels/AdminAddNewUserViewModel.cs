using System.ComponentModel.DataAnnotations;

namespace dafsem.Models.ViewModels
{
    public class AdminAddNewUserViewModel
    {
        [Key]
        public required string Id { get; set; }
        [Required(ErrorMessage = "İsim alanı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "İsim en fazla 50 karakter uzunluğunda olabilir.")]
        [Display(Name = "Admin Adı")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Şifre en az 8 ve en fazla 40 karakter olmalıdır.")]
        [Display(Name = "Şifre")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Şifre doğrulama alanı boş bırakılamaz.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifreyi Onayla")]
        public required string ConfirmPassword { get; set; }
    }
}
