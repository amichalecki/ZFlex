using System.Text.Json.Serialization;

namespace ZFlex.Models
{
    public class AuthResponse
    {
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
        [JsonPropertyName("expires")]
        public int Expires { get; set; }
    }
}
