using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dafsem.Models
{
    public class SayfaBilesen
    {
        [Key]
        public int Id { get; set; }
        public int? BilesenId { get; set; }
        public int? EkSayfaId { get; set; }
        [ForeignKey(nameof(EkSayfaId))]
        public virtual EkSayfalar? EkSayfa { get; set; }
        public virtual List<SayfaBilesenDegerleri> SayfaBilesenDegerleri { get; set; }

        [DefaultValue(999)]
        public int Sira { get; set; } = 999;
        [ForeignKey("Dil")]
        public int DilId { get; set; }
        public virtual Dil? Dil { get; set; } = null;
        // Aktif olma durumu
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity

    }
}
