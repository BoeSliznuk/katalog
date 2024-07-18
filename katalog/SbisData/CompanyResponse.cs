using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class CompanyResponse
    {
        [JsonPropertyName("companies")]
        public List<Company>? Companies { get; set; }
        [JsonPropertyName("outcome")]
        public Outcome? Outcome { get; set; }
    }
}
