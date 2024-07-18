using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class ProductResponse
    {
        [JsonPropertyName("nomenclatures")]
        public List<Product>? Products { get; set; }
        [JsonPropertyName("outcome")]
        public Outcome? Outcome { get; set; }
    }
}
