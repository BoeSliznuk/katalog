using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class BalanceResponse
    {
        [JsonPropertyName("balances")]
        public List<Balances> Balances { get; set; }
    }
}
