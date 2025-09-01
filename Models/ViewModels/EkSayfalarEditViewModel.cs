namespace dafsem.Models.ViewModels
{
    public class EkSayfalarEditViewModel
    {
        public int Id { get; set; }
        public string SayfaBasligi { get; set; }
        public string Url { get; set; }
        public int? BulunduguSayfaId { get; set; }
        public List<BilesenViewModel> Bilesenler { get; set; } = new List<BilesenViewModel>();
    }
}
