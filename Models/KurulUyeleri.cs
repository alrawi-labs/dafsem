using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("KurulUyeleri")]
    public class KurulUyeleri
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Adı alanı zorunludur.")]
        [StringLength(255, ErrorMessage = "Adı en fazla 255 karakter olabilir.")]
        [DisplayName("Adı")]
        public required string Adi { get; set; }

        [Required(ErrorMessage = "Soyadı alanı zorunludur.")]
        [StringLength(255, ErrorMessage = "Soyadı en fazla 255 karakter olabilir.")]
        [DisplayName("Soyadı")]
        public required string Soyadi { get; set; }

        [StringLength(50, ErrorMessage = "Ünvan en fazla 50 karakter olabilir.")]
        [DisplayName("Ünvan")]
        public Unvan? Unvan { get; set; }

        [StringLength(255, ErrorMessage = "Kurum bilgisi en fazla 255 karakter olabilir.")]
        [DisplayName("Bağlı Olduğu Kurum")]
        public string? Kurum { get; set; }

        [Required(ErrorMessage = "Kategori bilgisi zorunludur.")]
        [DisplayName("Kurul Kategorisi")]
        public required KurulKategorileri KategoriId { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
