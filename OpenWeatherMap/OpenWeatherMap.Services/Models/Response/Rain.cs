using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models.Response
{
    public class Rain
    {
        [JsonPropertyName("1h")]
        public float? OneHour { get; set; }

        [JsonPropertyName("3h")]
        public float? ThreeHours { get; set; }
    }
}
