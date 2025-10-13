using System.Text.Json.Serialization;

namespace LoginAuthToken.Shared.Models
{
    public class IpConfigModel
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = default!;
    }

}