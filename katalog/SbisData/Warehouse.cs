using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class Warehouse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
