using Microsoft.AspNetCore.Mvc;
using OpenWeatherMap.Services.Interfaces;
using OpenWeatherMap.Services.Models;
using OpenWeatherMap.Services.Models.WeatherData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace OpenWeatherMap.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        IWeatherDataService _weatherDataService;
        IMemoryCache _memoryCache;
        public WeatherController(IWeatherDataService weatherDataService, IMemoryCache cacheProvider)
        {
            _weatherDataService = weatherDataService;
            _memoryCache = cacheProvider;
        }

        /// <summary>
        ///     Get all country list.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///     GET weather/country
        /// 
        /// </remarks>
        [HttpGet("country")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountryAsync(CancellationToken cancellationToken = default)
        {
            if (!_memoryCache.TryGetValue("_Countries", out IEnumerable<Country> countries))
            {
                countries = await _weatherDataService.GetAllCountryAsync(cancellationToken);
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                    Size = 1024,
                };
                _memoryCache.Set("_Countries", countries, cacheEntryOptions);
            }
            return Ok(countries);
        }

        /// <summary>
        ///     Get all city list by country code.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///     GET weather/city/UK
        /// 
        /// </remarks>
        [HttpGet("city/{countryCode}")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCityByCountryCodeAsync(string countryCode, CancellationToken cancellationToken = default)
        {
            if (!_memoryCache.TryGetValue("_Cities", out IEnumerable<City> cities))
            {
                cities = await _weatherDataService.GetAllCityAsync(cancellationToken);
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                    Size = 1024,
                };
                _memoryCache.Set("_Cities", cities, cacheEntryOptions);
            }
            return Ok(cities.Where(x => x.Country.ToUpper() == countryCode.ToUpper()).AsEnumerable<City>());
        }

        /// <summary>
        ///     Get weather by city name and country code.
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///     POST weather
        /// 
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<WeatherData>> GetWeatherAsync(WeatherPayload weatherPayload, CancellationToken cancellationToken = default)
        {
            return Ok(await _weatherDataService.GetWeatherAsync(weatherPayload, cancellationToken));
        }
    }
}
