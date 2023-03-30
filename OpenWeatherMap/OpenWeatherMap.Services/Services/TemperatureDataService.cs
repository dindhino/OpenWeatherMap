using OpenWeatherMap.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap.Services.Services
{
    public class TemperatureDataService : ITemperatureDataService
    {
        public async Task<float> KelvinToFahrenheit(float TemperatureInKelvin, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                return await Task.Run(() => ((TemperatureInKelvin - 273.15f) * 1.8f) + 32);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<float> FahrenheitToCelcius(float TemperatureInFahrenheit, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                return await Task.Run(() => (TemperatureInFahrenheit - 32) / 1.8f);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
