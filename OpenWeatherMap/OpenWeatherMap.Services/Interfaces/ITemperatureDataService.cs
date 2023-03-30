using OpenWeatherMap.Services.Models;
using OpenWeatherMap.Services.Models.WeatherData;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap.Services.Interfaces
{
    public interface ITemperatureDataService
    {
        Task<float> KelvinToFahrenheit(float TemperatureInKelvin, CancellationToken cancellationToken = default);
        Task<float> FahrenheitToCelcius(float TemperatureInFahrenheit, CancellationToken cancellationToken = default);
    }
}
