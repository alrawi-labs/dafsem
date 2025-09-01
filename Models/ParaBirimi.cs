using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("ParaBirimleri")]
    public class ParaBirimi
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Para birimi kodu gereklidir.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Para birimi kodu 3 karakter olmalıdır.")]
        [Display(Name = "Para Birimi Kodu")]
        public required string Kod { get; set; } // Örneğin: "USD", "EUR", "TRY"

        [Required(ErrorMessage = "Para birimi adı gereklidir.")]
        [StringLength(50, ErrorMessage = "Para birimi adı en fazla 50 karakter olabilir.")]
        [Display(Name = "Para Birimi Adı")]
        public required string Ad { get; set; } // Örneğin: "Amerikan Doları", "Euro", "Türk Lirası"

        [Required(ErrorMessage = "Sembol gereklidir.")]
        [StringLength(5, ErrorMessage = "Sembol en fazla 5 karakter olabilir.")]
        [Display(Name = "Para Birimi Sembolü")]
        public required string Sembol { get; set; } // Örneğin: "$", "€", "₺"

        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;
    }
}
