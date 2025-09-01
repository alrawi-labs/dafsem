using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Kurallar")]
    public class Kurallar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kural metni gereklidir.")]
        [DisplayName("Kural Metni")]
        [StringLength(500, ErrorMessage = "Kural metni en fazla 500 karakter olabilir.")]
        public required string Metin { get; set; }
        [ForeignKey(nameof(KuralTuru))]
        [Display(Name = "Kural Türü ID")]
        public int KuralTuruId { get; set; }

        [Required(ErrorMessage = "Kural türü gereklidir.")]
        [DisplayName("Kural Türü")]
        public virtual KuralTuru Turu { get; set; }
        [ForeignKey("Dil")]
        public int DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
