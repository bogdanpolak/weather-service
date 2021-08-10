using System.Threading.Tasks;
using WeatherService.Domain.Model;

namespace WeatherService.Contracts
{
    public interface ILocationInfoRepository
    {
        Task<LocationInfo> GetLocationInfo(string locationCode);
        Task<string[]> GetLocations();
    }
}