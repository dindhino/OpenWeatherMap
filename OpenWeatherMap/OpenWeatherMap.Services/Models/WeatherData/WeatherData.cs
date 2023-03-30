using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models.WeatherData
{
    public class WeatherData
    {
        [JsonPropertyName("location")]
        public string Location { get; set; }
        
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("wind")]
        public float Wind { get; set; }

        [JsonPropertyName("visibility")] 
        public int Visibility { get; set; }
        
        [JsonPropertyName("skyConditions")]
        public string SkyConditions { get; set; }
        
        [JsonPropertyName("temperatureInCelcius")]
        public float TemperatureInCelsius { get; set; }
        
        [JsonPropertyName("temperatureInFahrenheit")]
        public float TemperatureInFahrenheit { get; set; }
        
        [JsonPropertyName("dewPoint")]
        public int DewPoint { get; set; }
        
        [JsonPropertyName("relativeHumidity")]
        public int RelativeHumidity { get; set; }
        
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }
    }
}
