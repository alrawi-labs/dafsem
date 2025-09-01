using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Sayfalar")]
    public class Sayfalar
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Sayfa başlığı en fazla 500 karakter olabilir.")]
        [DisplayName("Sayfa Başlığı")]
        public required string SayfaBasligi { get; set; }
        [DisplayName("Alt Sayfaları")]
        public ICollection<AltSayfa>? AltSayfalari { get; set; }
        public required string Url { get; set; }
        public int? AyarlarId { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
