using System.Text.Json.Serialization;

namespace katalog.SbisData
{
    public class AuthRequest
    {
        [JsonPropertyName("app_client_id")]
        public string? ClientId {  get; set; }
        [JsonPropertyName("app_secret")]
        public string? AppSecret {  get; set; }
        [JsonPropertyName("secret_key")]
        public string? SecretKey {  get; set; }
    }
}
