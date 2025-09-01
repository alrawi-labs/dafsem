using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("KurulKategorileri")]

    public class KurulKategorileri
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori başlığı zorunludur.")]
        [StringLength(255, ErrorMessage = "Kategori başlığı en fazla 255 karakter olabilir.")]
        [DisplayName("Kategori Başlığı")]
        public required string Baslik { get; set; }

        public ICollection<KurulUyeleri>? KurulUyeleri { get; set; }

        [DisplayName("Görüntülenme Sırası")]
        [Range(1, 1000, ErrorMessage = "{0} 1-1000 arasında olmalıdır.")]
        [DefaultValue(999)]
        public int Sira { get; set; } = 999;

        [DisplayName("Bulunduğu Sayfa")]
        [ForeignKey(nameof(Sayfa))] // Modern C# reference
        public required int SayfaId { get; set; }

        [JsonIgnore] // Prevents serialization issues
        public virtual AltSayfa Sayfa { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
 