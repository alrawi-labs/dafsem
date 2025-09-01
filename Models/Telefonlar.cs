using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Telefonlar")]
    public class Telefonlar
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        [DisplayName("Telefon Numarası")]
        [StringLength(30, ErrorMessage = "Telefon numarası en fazla 30 karakter olabilir.")]
        public required string Tel { get; set; }

        // Normalizasyon kurallarına aykırı olmasına rağmen, veri miktarının az olacağını göz önünde bulundurarak, ayrı bir tablo oluşturmanın gereksiz olduğunu düşündüm ve bu şekilde implement ettim.
        [DisplayName("Dahili")]
        [StringLength(50, ErrorMessage = "Dahili numaralar en fazla 50 karakter olabilir.")]
        [RegularExpression(@"^(\d{1,5}\/?)+$", ErrorMessage = "Dahili numaralar yalnızca sayılardan oluşmalı.")]
        public string? Dahili { get; set; }

        // Foreign key for Iletisim
        [ForeignKey(nameof(Iletisim))]  // Explicit foreign key relationship
        public int IletisimId { get; set; }

        // Navigation property
        [Required(ErrorMessage = "Iletişim bilgileri.")]
        [Display(Name ="İletisim")]
        public virtual Iletisim Iletisim { get; set; }

        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        // Aktif olma durumu
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
