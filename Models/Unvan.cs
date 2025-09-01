using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore; // [Index] attribute için eklenmeli

namespace dafsem.Models
{
    [Table("Unvan")]
    public class Unvan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Ünvan", Prompt = "Akademik ünvan giriniz")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} {2}-{1} karakter aralığında olmalıdır.")]
        [Column(TypeName = "nvarchar(50)")]
        public string UnvanAdi { get; set; } = null!;

        [DisplayName("Görüntülenme Sırası")]
        [Range(1, 1000, ErrorMessage = "{0} 1-1000 arasında olmalıdır.")]
        [DefaultValue(999)]
        public int Sira { get; set; } = 999;
        [ForeignKey("Dil")]
        public int? DilId { get; set; }
        [JsonIgnore]
        public virtual Dil? Dil { get; set; } = null;
        public bool State { get; set; } = true;
    }
}
