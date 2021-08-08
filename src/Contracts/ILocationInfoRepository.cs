using WeatherService.Domain.Model;
using WeatherService.Repositories;

namespace WeatherService.Contracts
{
    public interface ILocationInfoRepository
    {
        LocationInfo GetLocationInfo(string locationCode);
        string[] GetLocations();
    }
}