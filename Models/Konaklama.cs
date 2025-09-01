using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("Konaklama")]
    public class Konaklama
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Konaklama evi gereklidir.")]
        [DisplayName("Konaklama Evi")]
        [StringLength(255, ErrorMessage = "Konaklama evi adı en fazla 255 karakter olabilir.")]
        public required string KonaklamaEvi { get; set; }

        [Required(ErrorMessage = "Adres gereklidir.")]
        [DisplayName("Adres")]
        public required string Adres { get; set; }

        [DisplayName("Telefon Numarası")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir.")]

        public string? Tel { get; set; }


        [DisplayName("E-posta")]
        [StringLength(100, ErrorMessage = "Eposta en fazla 100 karakter olabilir.")]

        public string? Eposta { get; set; }


        [DisplayName("Web Sitesi")]
        [StringLength(255, ErrorMessage = "Web site en fazla 255 karakter olabilir.")]

        public string? WebSitesi { get; set; }

        [DisplayName("Yıldız Sayısı")]
        public int? YildizSayisi { get; set; }

        [DisplayName("Kahvaltı Dahil Mi ?")]
        public bool? KahvaltiDahilMi { get; set; }

        public IEnumerable<OdaTipleri> Odalar { get; set; }

        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;
    }
}
