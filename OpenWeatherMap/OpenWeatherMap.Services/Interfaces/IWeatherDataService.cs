using OpenWeatherMap.Services.Models;
using OpenWeatherMap.Services.Models.WeatherData;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap.Services.Interfaces
{
    public interface IWeatherDataService
    {
        Task<IEnumerable<Country>> GetAllCountryAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<City>> GetAllCityAsync(CancellationToken cancellationToken = default);
        Task<WeatherData> GetWeatherAsync(WeatherPayload weatherPayload, CancellationToken cancellationToken = default);
    }
}
