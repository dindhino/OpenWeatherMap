using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models.Common
{
    public class Coord
    {
        [JsonPropertyName("lon")]
        public float Lon { get; set; }
        
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
    }
}
