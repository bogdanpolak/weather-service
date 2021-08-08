using System;

namespace WeatherService.Domain.Model
{
    public class DayForecast
    {
        public DateTime Date { get; set; }
        public float Temperature { get; set; }
        public string Summary { get; set; }
    }
}