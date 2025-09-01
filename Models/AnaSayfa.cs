using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("AnaSayfa")]
    public class AnaSayfa
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Ana Sayfadaki Gösterilen Fotoğraflar")]
        public ICollection<Fotolar>? Sliderler { get; set; }
        [DisplayName("Afiş")]
        public Fotolar? AfiseId { get; set; }
        [StringLength(5000)]
        [DisplayName("Mektup")]
        public string? Mektup { get; set; }
        [ForeignKey("Dil")]
        public int DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
