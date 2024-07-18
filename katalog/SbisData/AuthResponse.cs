using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        [JsonPropertyName("sid")]
        public string? Sid { get; set; }
        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
