using Microsoft.Extensions.Configuration;
using OpenWeatherMap.Services.Interfaces;
using OpenWeatherMap.Services.Models;
using OpenWeatherMap.Services.Models.Response;
using OpenWeatherMap.Services.Models.WeatherData;
using OpenWeatherMap.Services.Services.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap.Services.Services
{
    public class WeatherDataService : IWeatherDataService
    {
        readonly IConfiguration _configuration;
        readonly string _ApiBaseUrl;
        readonly string _GetWeatherEndpoint;
        readonly string _ApiKey;
        readonly HttpClient _httpClient;
        readonly ITemperatureDataService _temperatureDataService;
        public WeatherDataService(IConfiguration configuration, ITemperatureDataService temperatureDataService)
        {
            _configuration = configuration;
            _ApiBaseUrl = _configuration.GetValue<string>("OpenWeatherMapSettings:ApiBaseUrl");
            _GetWeatherEndpoint = _configuration.GetValue<string>("OpenWeatherMapSettings:GetWeatherEndpoint");
            _ApiKey = _configuration.GetValue<string>("OpenWeatherMapSettings:ApiKey");
            _httpClient = new HttpClient();
            _temperatureDataService = temperatureDataService;
        }

        public async Task<IEnumerable<Country>> GetAllCountryAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string filePath = $"{Environment.CurrentDirectory}\\Data\\country.list.json";
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string json = await reader.ReadToEndAsync();
                    return JsonSerializer.Deserialize<IEnumerable<Country>>(json);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<City>> GetAllCityAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string filePath = $"{Environment.CurrentDirectory}\\Data\\city.list.json";
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string json = await reader.ReadToEndAsync();
                    return JsonSerializer.Deserialize<IEnumerable<City>>(json);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WeatherData> GetWeatherAsync(WeatherPayload weatherPayload, CancellationToken cancellationToken = default)
        {
            try
            {
                await weatherPayload.ValidateAsync<WeatherPayload.Validator>(cancellationToken);
                string url = $"{_ApiBaseUrl}{_GetWeatherEndpoint}?q={weatherPayload.CityName},{weatherPayload.CountryCode}&appid={_ApiKey}";
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url)
                };

                var responseMessage = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (responseMessage.IsSuccessStatusCode)
                {
                    WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(response);
                    return await MapToWeatherData(weatherResponse);
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task<WeatherData> MapToWeatherData(WeatherResponse weatherResponse)
        {
            float temperatureInFahrenheit = await _temperatureDataService.KelvinToFahrenheit(weatherResponse.Main.Temp);
            float temperatureInCelsius = await _temperatureDataService.FahrenheitToCelcius(temperatureInFahrenheit);
            WeatherData weatherData = new WeatherData()
            {
                Location = weatherResponse.Name,
                Time = DateTime.UtcNow,
                Wind = weatherResponse.Wind.Speed,
                Visibility = weatherResponse.Visibility,
                SkyConditions = weatherResponse.Weather.FirstOrDefault().Description ?? "",
                TemperatureInFahrenheit = temperatureInFahrenheit,
                TemperatureInCelsius = temperatureInCelsius,
                DewPoint = weatherResponse.Clouds.All,
                RelativeHumidity = weatherResponse.Main.Humidity,
                Pressure = weatherResponse.Main.Pressure,

            };
            return weatherData;
        }
    }
}
