using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class Company
    {
        [JsonPropertyName("companyId")]
        public int Id { get; set; }
    }
}
