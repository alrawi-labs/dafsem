using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Basvuru")]
    public class Basvuru
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Başvuru Sayfasında Üst Metin")]
        [StringLength(1500, ErrorMessage = "Üst metin en fazla 1500 karakter olabilir.")]
        public string? UstMetin { get; set; }

        [DisplayName("Başvuru Formu")]
        [StringLength(200, ErrorMessage = "Başvuru Formu Yolu en fazla 200 karakter olabilir.")]
        public string? Form { get; set; }

        [DisplayName("Başvuru Sayfasında Alt Metin")]
        [StringLength(1500, ErrorMessage = "Alt metin en fazla 1500 karakter olabilir.")]
        public string? AltMetin { get; set; }
        [ForeignKey("Dil")]
        public int DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
