using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    [Table("Iletisim")]
    public class Iletisim
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Eposta")]
        [StringLength(150, ErrorMessage = "E-Posta en fazla 150 karakter olabilir.")]
        public string? Eposta { get; set; }

        [DisplayName("Adres")]
        [StringLength(750, ErrorMessage = "Adres en fazla 750 karakter olabilir.")]
        public string? Adres { get; set; }

        [DisplayName("Telefon")]
        public virtual ICollection<Telefonlar> Telefonlar { get; set; } = new List<Telefonlar>();

        [ForeignKey("Dil")]
        public int DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;

        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity


    }
}
