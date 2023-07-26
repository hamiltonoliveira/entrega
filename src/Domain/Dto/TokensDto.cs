using System.Text.Json.Serialization;

namespace Domain.Dto
{
    public class TokensDto
    {
        public string? Token { get; set; }
        public string? TokenRefresh { get; set; }

        [JsonIgnore]
        public string? Role { get; set; }
    }
}
