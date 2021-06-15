using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZDC.Core.Auth
{
    public class TokenResponse
    {
        [JsonPropertyName("scopes")] public IList<string> Scopes { get; set; }
        [JsonPropertyName("token_type")] public string TokenType { get; set; }
        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
        [JsonPropertyName("access_token")] public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }
    }
}