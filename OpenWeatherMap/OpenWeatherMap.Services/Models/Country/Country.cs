using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models
{
    public class Country
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
