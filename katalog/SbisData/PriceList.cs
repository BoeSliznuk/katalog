using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class PriceList
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
