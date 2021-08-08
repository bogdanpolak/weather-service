using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherService.Contracts;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger; 
        private readonly ILocationInfoRepository _locationInfoRepository;

        public LocationController(
            ILogger<LocationController> logger, 
            ILocationInfoRepository locationInfoRepository)
        {
            _logger = logger;
            _locationInfoRepository = locationInfoRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation($"[GET /location]");
            var locations = _locationInfoRepository.GetLocations();
            return Ok(locations);
        }
    }
}