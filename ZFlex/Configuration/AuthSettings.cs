using System.Text.Json.Serialization;

namespace ZFlex.Configuration
{
    public class AuthSettings
    {
        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; set; }
        [JsonPropertyName("grant_type")]
        public required string GrantType { get; set; }
    }
}
