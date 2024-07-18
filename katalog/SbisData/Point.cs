using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class Point
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
