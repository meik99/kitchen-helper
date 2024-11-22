using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace website.Account {
    public class TokenAuthenticationResponse {
        [JsonPropertyName("user")]
        public User? User { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("exp")]
        public long Exp { get; set; }
    }

    public class User {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public String? Email { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
