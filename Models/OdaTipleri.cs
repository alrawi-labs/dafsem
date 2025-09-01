using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("OdaTipleri")]
    public class OdaTipleri
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Konaklama evi bilgisi gereklidir.")]
        [DisplayName("Konaklama Evi")]
        public required Konaklama KonaklamaEvi { get; set; }

        [Required(ErrorMessage = "Oda tipi bilgisi gereklidir.")]
        [StringLength(100, ErrorMessage = "Oda tipi en fazla 100 karakter olabilir.")]
        [DisplayName("Oda Tipi")]
        public required string OdaTipi { get; set; }

        [DisplayName("Ücret")]
        [Range(0, float.MaxValue, ErrorMessage = "Ücret sıfır veya daha büyük bir değer olmalıdır.")]
        public float? Ucret { get; set; }

        [DisplayName("Para Birimi")]
        public ParaBirimi? Birim { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;
    }
}
