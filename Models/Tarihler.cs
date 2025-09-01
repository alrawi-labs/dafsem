using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Tarihler")]
    public class Tarihler
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Tarih")]
        public required DateTime Tarih { get; set; }
        [Required]
        [DisplayName("Açıklama")]
        [StringLength(255, ErrorMessage = "Açıklama en fazla 255 karakter olabilir.")]
        public required string Aciklama { get; set; }

        [ForeignKey("Dil")]
        public int DilId { get; set; }

        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;

    }
}