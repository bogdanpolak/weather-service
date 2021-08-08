using System;
using System.Linq;

namespace WeatherService.Domain.Model
{
    public class LocationInfo
    {
        public string LocationCode { get; }
        public TemperatureRange[] MeanTemperatures { get; }
        
        public LocationInfo(string locationCode, string temperatures)
        {
            var meanTemperatures = temperatures.Split(",");
            if (!(meanTemperatures is {Length: 12}))
                throw new Exception("Mean temperatures requires 12 months data");
            LocationCode = locationCode;
            MeanTemperatures = meanTemperatures.ToList().Select( (range, idx) =>
            {
                var tempRange = range.Split(" ");
                if (!(tempRange is {Length: 2}))
                    throw new Exception($"Invalid temperature range for {locationCode}, month {idx+1}");
                if (!int.TryParse(tempRange[0],  out var minTemp))
                    throw new Exception($"Invalid temperature range for {locationCode}, month {idx+1}");
                if (!int.TryParse(tempRange[1],  out var maxTemp))
                    throw new Exception($"Invalid temperature range for {locationCode}, month {idx+1}");
                return new TemperatureRange(minTemp,maxTemp);
            }).ToArray();
        }
    }
}