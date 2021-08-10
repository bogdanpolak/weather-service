using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeatherService.Contracts;
using WeatherService.Domain.Model;

namespace WeatherService.Features
{
    public static class GenerateWeatherFeature
    {
        public class Query : IRequest<WeatherForecast>
        {
            public string LocationCode;
            public int DaysToGenerate;
        }
        
        public class QueryHandler : IRequestHandler<Query, WeatherForecast>
        {
            private const int MaximumDays = 19;
            
            private readonly ILocationInfoRepository _locationInfoRepository;

            public QueryHandler(ILocationInfoRepository locationInfoRepository)
            {
                _locationInfoRepository = locationInfoRepository;
            }

            public async Task<WeatherForecast> Handle(Query request, CancellationToken cancellationToken)
            {
                return await Generate(request.LocationCode, request.DaysToGenerate);
            }
            
            private async Task<WeatherForecast> Generate(string locationCode, int days)
            {
                var locationInfo = await _locationInfoRepository.GetLocationInfo(locationCode);
                if (days > MaximumDays)
                    throw new GenerateDaysOutOfRangeException(days, MaximumDays);
                if (locationInfo == null)
                    throw new InvalidLocationCodeException(locationCode);
                var random = new Random();
                var meanMonthTemp = locationInfo.MeanTemperatures[DateTime.Now.Month - 1];
                var dayForecasts = Enumerable.Range(1, days)
                    .Select(index => new DayForecast 
                    {
                        Date = DateTime.Now.AddDays(index).Date,
                        Temperature = random.Next(meanMonthTemp.Minimal, meanMonthTemp.Maximum),
                        Summary = GetRandomSummary(random, rainChancePercent: 20)
                    })
                    .ToList();
                var weatherForecast = new WeatherForecast
                {
                    LocationCode = locationCode,
                    Generated = DateTimeOffset.Now,
                    TemperatureScale = "celsius" // fahrenheit : 32 + (int)(celsius / 0.5556);
                };
                weatherForecast.Days.AddRange(dayForecasts);
                return weatherForecast;
            }

            private string GetRandomSummary(Random random, int rainChancePercent)
            {
                return (random.Next(100) < rainChancePercent)
                    ? WeatherData.RainSummaries[random.Next(WeatherData.RainSummaries.Length)]
                    : WeatherData.Summaries[random.Next(WeatherData.Summaries.Length)];
            }
        }

        public class InvalidLocationCodeException : Exception
        {
            public InvalidLocationCodeException(string location)
                : base($"Invalid location '{location}'. System has no information for that location")
            { }
        }
        
        public class GenerateDaysOutOfRangeException : Exception
        {
            public GenerateDaysOutOfRangeException(int daysToGenerate, int maximumDays)
                : base($"Generate days out of range. Requested: {daysToGenerate}. " +
                       $"Forecast can be generated for maximum {maximumDays} days")
            { }
        }

        private static class WeatherData
        {
            public static readonly string[] Summaries = new[]
            {
                "cloudy", "mostly cloudy", "broken clouds", "scattered clouds", "partly sunny", "mostly sunny", "sunny"
            };
            public static readonly string[] RainSummaries = new[]
            {
                "showers, broken clouds", "light showers, overcast", "sprinkles, overcast", "rain, cloudy", "heavy rain, cloudy"
            };
        } 
    }
}