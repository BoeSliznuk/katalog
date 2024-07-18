using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class Balances
    {
        [JsonPropertyName("balance")]
        public double Balance { get; set; }
        [JsonPropertyName("nomenclature")]
        public int Nomenclature { get; set; }
    }
}
