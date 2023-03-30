using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenWeatherMap.Services.Models;
using OpenWeatherMap.Services.Models.WeatherData;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenWeatherMap.Test
{
    [TestClass]
    public class WeatherControllerTest
    {
        private static readonly HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44376") };

        public WeatherControllerTest()
        {

        }

        public void Dispose()
        {
            _httpClient.DeleteAsync("/state").GetAwaiter().GetResult();
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("DummyCityName", "XCountryCodeX")]
        public async Task Post_GetWeather_NegativeData(string cityName, string countryCode)
        {
            var expectedStatusCode = System.Net.HttpStatusCode.InternalServerError;
            var jsonMediaType = "application/json";
            var stringContent = new StringContent
                                (
                                    JsonConvert.SerializeObject(new WeatherPayload() { CityName = cityName, CountryCode = countryCode }),
                                    Encoding.UTF8,
                                    jsonMediaType
                                );
            var response = await _httpClient.PostAsync("/weather", stringContent);
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }
    }
}
