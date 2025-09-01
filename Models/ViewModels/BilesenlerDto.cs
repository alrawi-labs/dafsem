namespace dafsem.Models.ViewModels
{
    public class BilesenlerDto
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string[] Parametreler { get; set; }
        public string[] ParametrelerTipleri { get; set; }
    }
}
