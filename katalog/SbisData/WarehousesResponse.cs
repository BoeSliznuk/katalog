using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class WarehousesResponse
    {
        [JsonPropertyName("warehouses")]
        public List<Warehouse>? Warehouses { get; set; }
    }
}
