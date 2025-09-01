using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace dafsem.Models
{
    [Table("BankaBilgileri")]
    public class BankaBilgileri
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Banka adı gereklidir.")]
        [DisplayName("Banka Adı")]
        [StringLength(255, ErrorMessage = "Banka adı en fazla 255 karakter olabilir.")]
        public required string BankaAdi { get; set; }

        [Required(ErrorMessage = "Hesap sahibi adı gereklidir.")]
        [DisplayName("Hesap Sahibi Adı")]
        [StringLength(255, ErrorMessage = "Hesap sahibi adı en fazla 255 karakter olabilir.")]
        public required string HesapSahibiAdi { get; set; }

        [Required(ErrorMessage = "IBAN numarası gereklidir.")]
        [DisplayName("IBAN Numarası")]
        [StringLength(34, ErrorMessage = "IBAN numarası en fazla 34 karakter olabilir.")]
        public required string IBAN { get; set; }

        public string FormatIBAN(string iban)
        {
            
            string normalizedIban = Regex.Replace(iban.ToUpper(), @"\s+", "");

            
            if (!Regex.IsMatch(normalizedIban, @"^[A-Z]{2}\d{2}[A-Z0-9]{1,30}$"))
            {
                throw new ValidationException("Geçerli bir IBAN numarası giriniz.");
            }

            return Regex.Replace(normalizedIban, @"(.{4})(?!$)", "$1 ");
        }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;
    }
}
