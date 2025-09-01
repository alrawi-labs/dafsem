namespace dafsem.Models.ViewModels
{
    public class BilesenViewModel
    {
        public int SayfaBilesenId { get; set; }
        public int Index { get; set; }
        public int DataId { get; set; }
        public string Baslik { get; set; }
        public string Content { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public List<string> Ids { get; set; } = new List<string>();
        public List<int> DataIds { get; set; } = new List<int>();
    }
}
