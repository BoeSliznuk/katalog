using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class PointResponse
    {
        [JsonPropertyName("salesPoints")]
        public List<Point>? Points { get; set; }
        [JsonPropertyName("outcome")]
        public Outcome? Outcome {  get; set; } 
    }
}
