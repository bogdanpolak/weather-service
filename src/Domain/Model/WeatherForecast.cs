using System;
using System.Collections.Generic;

namespace WeatherService.Domain.Model
{
    public class WeatherForecast
    {
        public string LocationCode { get; set; }
        public DateTimeOffset Generated { get; set; }
        public string TemperatureScale { get; set; }
        public List<DayForecast> Days { get; } = new List<DayForecast>();
    }
}
