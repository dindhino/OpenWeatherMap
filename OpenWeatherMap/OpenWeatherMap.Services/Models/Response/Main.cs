using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models.Response
{
    public class Main
    {
        [JsonPropertyName("temp")]
        public float Temp { get; set; }
        
        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }
        
        [JsonPropertyName("temp_min")]
        public float TempMin { get; set; }
        
        [JsonPropertyName("temp_max")] 
        public float TempMax { get; set; }
        
        [JsonPropertyName("pressure")] 
        public int Pressure { get; set; }
        
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
        
        [JsonPropertyName("sea_level")]
        public int SeaLevel { get; set; }
        
        [JsonPropertyName("grnd_level")]
        public int GrndLevel { get; set; }
    }
}
