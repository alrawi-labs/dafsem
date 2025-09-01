using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Ayarlar")]
    public class Ayarlar
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Site Adı")]
        [StringLength(50, ErrorMessage = "Site adı en fazla 50 karakter olabilir.")]
        public string? SiteAdi { get; set; }

        [DisplayName("Kurum Adı")]
        [StringLength(50, ErrorMessage = "Kurum adı en fazla 50 karakter olabilir.")]
        public string? KurumAdi { get; set; }

        // ✅ Site Logosu 
        public int? SiteLogoId { get; set; }
        [ForeignKey(nameof(SiteLogoId))] 
        [DisplayName("Site Logosu")]
        public virtual Fotolar? SiteLogo { get; set; }

        // ✅ Sağ Logo
        public int? SagLogoId { get; set; }
        [ForeignKey(nameof(SagLogoId))]
        [DisplayName("Sağ Taraftaki Logo")]
        public virtual Fotolar? SagLogo { get; set; }

        // ✅ Sol Logo
        public int? SolLogoId { get; set; }
        [ForeignKey(nameof(SolLogoId))]
        [DisplayName("Sol Taraftaki Logo")]
        public virtual Fotolar? SolLogo { get; set; }

        [DisplayName("Site Alt Başlığı")]
        [StringLength(255, ErrorMessage = "Site alt başlığı en fazla 255 karakter olabilir.")]
        public string? SiteAltBaslik { get; set; }

        // ✅ Filigran Fotoğrafı
        public int? FiligranId { get; set; }
        [ForeignKey(nameof(FiligranId))]
        [DisplayName("Filigran Fotoğrafı")]
        public virtual Fotolar? Filigran { get; set; }

        // ✅ Site Arkaplan Fotoğrafı
        public int? SiteArkaplaniId { get; set; }
        [ForeignKey(nameof(SiteArkaplaniId))]
        [DisplayName("Site Arkaplan Fotoğrafı")]
        public virtual Fotolar? SiteArkaplani { get; set; }

        [DisplayName("Program")]
        [StringLength(500, ErrorMessage = "Program yolu en fazla 500 karakter olabilir.")]
        public string? Program { get; set; }

        [DisplayName("Sayfa Başlıkları")]
        public ICollection<Sayfalar>? Sayfalar { get; set; }

        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;
    }
}
