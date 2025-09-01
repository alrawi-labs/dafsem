using System.Text.Json.Serialization;

namespace dafsem.Models.ViewModels
{
    public class EksayfaViewModel
    {
        public int? Id { get; set; }
        [JsonPropertyName("SayfaBasligi")]
        public string SayfaBasligi { get; set; }

        [JsonPropertyName("BulunduguSayfaId")]
        public int? BulunduguSayfaId { get; set; } // ❗️ Nullable yaptık

        [JsonPropertyName("Url")]
        public string Url { get; set; }

        [JsonPropertyName("Ekler")]
        public List<BilesenJsonViewModel> Ekler { get; set; }
    }
}
