using System.Text.Json.Serialization;

namespace server.Models
{
    /// <summary>
    /// Consumes Auth0 REST server response for token.
    /// </summary>
    public record AuthorizationTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("scope")]
        public string Scope { get; init; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; }
    }
}

