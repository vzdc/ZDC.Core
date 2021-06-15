using System.Text.Json.Serialization;

namespace ZDC.Core.Auth
{
    public class UserResponse
    {
        [JsonPropertyName("data")] public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("cid")] public string Cid { get; set; }
        [JsonPropertyName("personal")] public Personal Personal { get; set; }
        [JsonPropertyName("vatsim")] public Vatsim Vatsim { get; set; }
        [JsonPropertyName("oauth")] public Oauth2 Oauth { get; set; }
    }

    public class Personal
    {
        [JsonPropertyName("name_first")] public string FirstName { get; set; }
        [JsonPropertyName("name_last")] public string LastName { get; set; }
        [JsonPropertyName("name_full")] public string FullName { get; set; }
        [JsonPropertyName("email")] public string Email { get; set; }
    }

    public class Vatsim
    {
        [JsonPropertyName("rating")] public Rating Rating { get; set; }
    }

    public class Rating
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("long")] public string Long { get; set; }
        [JsonPropertyName("short")] public string Short { get; set; }
    }

    public class Oauth2
    {
        [JsonPropertyName("token_valid")] public string ValidToken { get; set; }
    }
}