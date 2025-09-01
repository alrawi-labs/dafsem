using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dafsem.Models
{
    [Table("HizmetTuru")]
    public class HizmetTuru
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet türü gereklidir.")]
        [DisplayName("Hizmet Türü")]
        [StringLength(500, ErrorMessage = "Hizmet türü en fazla 500 karakter olabilir.")]
        public required string Tur { get; set; }
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;
    }
}
