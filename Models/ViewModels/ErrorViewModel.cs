using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace dafsem.Models.ViewModels
{
    public class ErrorViewModel
    {
        public int ErrorCode { get; set; }
    }

}
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace dafsem.Models
//{
//    [Table("FotoGruplar")]
//    public class FotoGruplar
//    {
//        [Key]
//        public int Id { get; set; }
//        [Required]
//        [DisplayName("Baþlýk")]
//        public required string Baslik { get; set; }
//        //public ICollection<Fotolar>? Fotolar { get; set; }

//    }
//}
