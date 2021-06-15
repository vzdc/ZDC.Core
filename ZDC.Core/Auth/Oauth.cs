using System.Text.Json.Serialization;

namespace ZDC.Core.Auth
{
    public class Oauth
    {
        [JsonPropertyName("grant_type")] public string GrantType => "authorization_code";

        [JsonPropertyName("client_id")] public string ClientId { get; set; }

        [JsonPropertyName("client_secret")] public string ClientSecret { get; set; }

        [JsonPropertyName("redirect_uri")] public string RedirectUri { get; set; }

        [JsonPropertyName("code")] public string Code { get; set; }
    }
}