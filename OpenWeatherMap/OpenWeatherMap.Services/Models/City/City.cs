using OpenWeatherMap.Services.Models.Common;
using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models
{
    public class City
    {
        [JsonPropertyName("id")]
        public decimal Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("state")]
        public string State { get; set; }
        
        [JsonPropertyName("country")]
        public string Country { get; set; }
        
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; }
    }
}
