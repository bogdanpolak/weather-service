using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherService.Contracts;
using WeatherService.Domain.Model;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "cloudy", "mostly cloudy", "broken clouds", "scattered clouds", "partly sunny", "mostly sunny", "sunny"
        };
        private static readonly string[] RainSummaries = new[]
        {
            "showers, broken clouds", "light showers, overcast", "sprinkles, overcast", "rain, cloudy", "heavy rain, cloudy"
        };


        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILocationInfoRepository _locationInfoRepository; 

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, 
            ILocationInfoRepository locationInfoRepository)
        {
            _logger = logger;
            _locationInfoRepository = locationInfoRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> Get(string location, int days)
        {
            var locationCode = location;
            _logger.LogInformation($"[GET /weatherforecast ? location={locationCode} & days={days}]");
            var locationInfo = _locationInfoRepository.GetLocationInfo(locationCode);
            if (days > 20)
                return BadRequest("Forecast can be generated for maximum 20 days");
            if (locationInfo == null)
                return NotFound();
            var random = new Random();
            var meanMonthTemp = locationInfo.MeanTemperatures[DateTime.Now.Month - 1];
            var dayForecasts = Enumerable.Range(1, days)
                .Select(index => new DayForecast 
                {
                    Date = DateTime.Now.AddDays(index).Date,
                    Temperature = random.Next(meanMonthTemp.Minimal, meanMonthTemp.Maximum),
                    Summary = (random.Next(5)!=0) ? Summaries[random.Next(Summaries.Length)]
                        : RainSummaries[random.Next(RainSummaries.Length)]
                })
                .ToList();
            var weatherForecast = new WeatherForecast
            {
                LocationCode = locationCode,
                Generated = DateTimeOffset.Now,
                TemperatureScale = "celsius" // fahrenheit : 32 + (int)(celsius / 0.5556);
            };
            weatherForecast.Days.AddRange(dayForecasts);
            return Ok(weatherForecast);
        }
    }
}
