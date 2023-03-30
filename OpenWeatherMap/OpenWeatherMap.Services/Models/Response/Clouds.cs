using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models.Response
{
    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
