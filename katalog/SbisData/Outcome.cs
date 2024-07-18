using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class Outcome
    {
        [JsonPropertyName("hasMore")]
        public bool? HasMore;
    }
}
