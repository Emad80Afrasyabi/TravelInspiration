using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using AutoMapper;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();
       
        services.AddSingleton<IMapper>(provider =>
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

            var config = new MapperConfiguration(configure: mapperConfigurationExpression =>
            {
                mapperConfigurationExpression.AddMaps(assembliesToScan: Assembly.GetExecutingAssembly());
            }, loggerFactory);

            return config.CreateMapper();
        });
        
        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TravelInspirationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(name: "TravelInspirationDbConnection")));
        return services;
    }
}
