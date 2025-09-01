using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dafsem.Models
{
    public class Dil
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Dil Adı")]
        [Required(ErrorMessage = "Dil adı zorunlu alandır.")]
        [StringLength(100, ErrorMessage = "{0} en fazla {1} karakter olmalıdır.")]
        public required string DilAdi { get; set; }

        [Display(Name = "Dil Kodu")]
        [Required(ErrorMessage = "Dil kodu zorunlu alandır.")]
        [StringLength(2, ErrorMessage = "{0} en fazla {1} karakter olmalıdır.")]
        public required string DilKodu { get; set; }

        // Aktif olma durumu
        [Column("State", TypeName = "bit")]
        public bool State { get; set; } = true;  // Renamed for clarity
    }
}
