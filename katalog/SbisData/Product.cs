using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("hierarchicalId")]
        public int? HierarchicalId { get; set; }
        [JsonPropertyName("hierarchicalParent")]
        public int? HierarchicalParent { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("nomNumber")]
        public string? NomNumber { get; set; }
        [JsonPropertyName("description_simple")]
        public string? DescriptionSimple { get; set; }
        [JsonPropertyName("images")]
        public List<string>? Images { get; set; }
        [JsonPropertyName("cost")]
        public double? Cost { get; set; }
        [JsonPropertyName("isParent")]
        public bool? IsParent { get; set; }
        [JsonPropertyName("published")]
        public bool Published { get; set; }


        public int? ProdCount { get; set; }
        
    }
}
