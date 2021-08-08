using Microsoft.Extensions.DependencyInjection;
using WeatherService.Contracts;
using WeatherService.Repositories;

namespace WeatherService.IoC
{
    public static class ServiceConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ILocationInfoRepository, LocationInfoRepository>();
        }
    }
}