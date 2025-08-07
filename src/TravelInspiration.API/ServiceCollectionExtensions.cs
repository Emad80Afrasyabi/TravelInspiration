using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using AutoMapper;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();
        
        services.RegisterSlices();
       
        var currentAssembly = Assembly.GetExecutingAssembly();
        
        services.AddSingleton<IMapper>(provider =>
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            
            var config = new MapperConfiguration(configure: mapperConfigurationExpression =>
            {
                mapperConfigurationExpression.AddMaps(assembliesToScan: currentAssembly);
            }, loggerFactory);

            return config.CreateMapper();
        });
        
        services.AddMediatR(serviceConfiguration =>
        {
            serviceConfiguration.RegisterServicesFromAssemblies(currentAssembly);
        });
        
        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TravelInspirationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(name: "TravelInspirationDbConnection")));
        return services;
    }
}
