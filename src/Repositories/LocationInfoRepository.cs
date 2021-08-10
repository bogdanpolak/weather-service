using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherService.Contracts;
using WeatherService.Domain.Model;

namespace WeatherService.Repositories
{
    public class LocationInfoRepository : ILocationInfoRepository
    {
        private static readonly LocationInfo[] _locationInfos = {
            new LocationInfo("usa/cleveland", "-6 1,-5 3,-1 8,5 15,10 21,15 26,18 28,17 27,13 23,7 17,3 10,-3 4"),
            new LocationInfo("usa/miami", "16 25,17 26,18 27,20 28,23 31,24 32,25 33,25 33,25 32,23 30,20 28,17 26"),
            new LocationInfo("usa/minneapolis", "-13 -5,-12 -3,-3 6,3 13,10 21,16 26,19 29,18 28,13 23,5 15,-2 6,-9 -2"),
            new LocationInfo("usa/new-york", "-3 4,-2 5,2 10,7 16,12 22,18 26,20 29,20 28,16 24,10 18,5 12,0 6"),
            new LocationInfo("usa/san-francisco", "8 14,9 16,9 17,10 17,11 18,12 19,12 19,13 20,13 21,12 21,10 17,8 14"),
        };

        public Task<LocationInfo> GetLocationInfo(string locationCode)
        {
            return Task.Run(() => _locationInfos
                .FirstOrDefault(l => l.LocationCode == locationCode));
        }

        public Task<string[]> GetLocations()
        {
            return Task.Run( () => _locationInfos
                .Select(l=>l.LocationCode).ToArray());
        }
    }
}