using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("Ucretler")]
    public class Ucretler
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir.")]
        [DisplayName("Başlık")]
        [StringLength(255, ErrorMessage = "Başlık en fazla 255 karakter olabilir.")]
        public required string Baslik { get; set; }

        [Required(ErrorMessage = "Ücret bilgisi gereklidir.")]
        [DisplayName("Ücret")]
        [Range(0, float.MaxValue, ErrorMessage = "Ücret sıfır veya daha büyük bir değer olmalıdır.")]
        public required float Ucret { get; set; }

        [Required(ErrorMessage = "Para Birimi gereklidir.")]
        [DisplayName("Para Birimi")]
        public required ParaBirimi Birim { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;

    }
}
