using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Basliklar")]
    public class Basliklar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Başlık")]
        [StringLength(500, ErrorMessage = "Başlık en fazla 500 karakter olabilir.")]
        public required string Baslik { get; set; }

        [ForeignKey("Dil")]
        public int DilId { get; set; } 
        public virtual Dil? Dil { get; set; } = null;

        public bool State { get; set; } = true;
    }
}
