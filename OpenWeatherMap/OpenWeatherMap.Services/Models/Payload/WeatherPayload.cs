using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenWeatherMap.Services.Models
{
    public class WeatherPayload : DataModel
    {
        [JsonPropertyName("cityName")]
        public string CityName { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }

        public class Validator : AbstractValidator<WeatherPayload>
        {
            public Validator()
            {
                RuleFor(p => p.CityName)
                    .NotNull()
                    .NotEmpty();
                RuleFor(p => p.CountryCode)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }
}
