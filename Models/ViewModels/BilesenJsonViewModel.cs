using System.Text.Json.Serialization;

namespace dafsem.Models.ViewModels
{
    public class BilesenJsonViewModel
    {
        [JsonPropertyName("values")]
        public List<string> Values { get; set; }

        [JsonPropertyName("ids")]
        public List<string> Ids { get; set; }

        [JsonPropertyName("dataIds")]
        public List<int> DataIds { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
