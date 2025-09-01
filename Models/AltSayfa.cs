using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("AltSayfa")]
    public class AltSayfa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Alt sayfa başlığı en fazla 255 karakter olabilir.")]
        [DisplayName("Alt Sayfa Başlığı")]
        public required string AltSayfaBaslik { get; set; }

        public int UstSayfaId { get; set; }

        [Required]
        [DisplayName("Üst Sayfa")]
        [ForeignKey(nameof(UstSayfaId))]
        public virtual Sayfalar UstSayfa { get; set; }

        [Required(ErrorMessage ="Url belirlemekte hata oluştu, yöneticilerle iletişime geçmeniz gerekmektedir")]
        public required string Url { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity


    }
}
