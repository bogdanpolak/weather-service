using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using WeatherService.Contracts;
using WeatherService.Domain.Model;
using WeatherService.Features;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, 
            ILocationInfoRepository locationInfoRepository, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<WeatherForecast>> Get(string location, int days)
        {
            _logger.LogInformation($"[GET /weatherforecast ? location={location} & days={days}]");
            try
            {
                var request = new GenerateWeatherFeature.Query
                {
                    LocationCode = location,
                    DaysToGenerate = days
                };
                var weatherForecast = _mediator.Send(request);
                return Ok(await weatherForecast);
            }
            catch (GenerateWeatherFeature.InvalidLocationCodeException exception)
            {
                _logger.LogError(exception, exception.Message);
                return NotFound(exception.Message);
            }
            catch (GenerateWeatherFeature.GenerateDaysOutOfRangeException exception)
            {
                _logger.LogError(exception, exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
