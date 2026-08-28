using System.Text.Json.Serialization;

namespace Gateway.Application.Dtos.Authentication
{
    public class AuthResultDto
    {
        public Guid Id { get; set; }

        public bool isAuthenticated { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> roles { get; set; }
        public string Message { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }

    }
}
