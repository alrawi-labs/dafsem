using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Fotolar")]
    public class Fotolar
    {
        [Key]
        public int Id { get; set; }
        //[DisplayName("Grup")]
        //public FotoGruplar? FotoGrupId { get; set; }
        [Required]
        public required string Yol { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
