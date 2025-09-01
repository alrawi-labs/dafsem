using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    public class SayfaBilesenDegerleri
    {
        [Key]
        public int Id { get; set; }
        public int? SayfaBilesenId { get; set; }
        [ForeignKey(nameof(SayfaBilesenId))]
        public virtual SayfaBilesen? SayfaBilesen { get; set; } = null;
        public string? Baslik { get; set; }
        public string? Deger { get; set; }
        [ForeignKey("Dil")]
        public int DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;
        // Aktif olma durumu
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity
    }
}
