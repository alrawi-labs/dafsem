using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("Hizmetler")]  // Explicit schema specification
    public class Hizmetler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet metni gereklidir.")]
        [StringLength(1500, MinimumLength = 10, ErrorMessage = "Hizmet metni 10-1500 karakter arasında olmalıdır.")]
        [Display(Name = "Hizmet Metni", Prompt = "Hizmet açıklamasını giriniz")]
        [Column("Hizmet", TypeName = "nvarchar(1500)")]
        public required string Hizmet { get; set; }

        [ForeignKey(nameof(HizmetTuru))]  // Explicit foreign key relationship
        [Display(Name = "Hizmet Türü ID")]
        public int HizmetTuruId { get; set; }

        [Required(ErrorMessage = "Hizmet türü gereklidir.")]
        [Display(Name = "Hizmet Türü")]
        public virtual HizmetTuru Turu { get; set; }  // Virtual for lazy loading

        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}