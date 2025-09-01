using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("KuralTuru")]
    public class KuralTuru
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Kural Türü")]
        [StringLength(255, ErrorMessage = "Kural türü en fazla 500 karakter olabilir.")]
        public required string Tur{ get; set; }

        [DisplayName("Bulunduğu Sayfa")]
        [ForeignKey(nameof(Sayfa))] // Modern C# reference
        public required int? SayfaId { get; set; }

        [JsonIgnore] // Prevents serialization issues
        public virtual AltSayfa? Sayfa { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

       

    }
}
