using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class PriceListResponse
    {
        [JsonPropertyName("priceLists")]
        public List<PriceList>? PriceLists { get; set; }
        [JsonPropertyName("outcome")]
        public Outcome? Outcome { get; set; }
    }
}
