using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Index(nameof(Url), IsUnique = true)]
    public class EkSayfalar
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Sayfa Başlığı")]
        public string? SayfaBasligi { get; set; }

        [Required]
        [Display(Name = "URL")]
        public string? Url { get; set; }

        [Display(Name = "Bulunduğu Sayfa")]
        public int? BulunduguSayfaId { get; set; }

        [ForeignKey(nameof(BulunduguSayfaId))]
        [Display(Name = "Bulunduğu Sayfa")]
        public virtual EkSayfalar? BulunduguSayfa { get; set; } = null;
        // 🌟 Alt Sayfalar Özelliği
        public virtual ICollection<EkSayfalar> AltSayfalar { get; set; } = new List<EkSayfalar>();

        public virtual List<SayfaBilesen> SayfaBilesenleri { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;
        // Aktif olma durumu
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity
    }
}
