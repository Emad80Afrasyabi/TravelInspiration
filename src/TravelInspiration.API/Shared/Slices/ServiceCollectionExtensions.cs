using System.Reflection;

namespace TravelInspiration.API.Shared.Slices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSlices(this IServiceCollection services)
    {
        // get current assembly
        var currentAssembly = Assembly.GetExecutingAssembly();

        // get slices 
        IEnumerable<Type> slices = currentAssembly.GetTypes().Where(type => typeof(ISlice).IsAssignableFrom(type) && 
                                                                            type != typeof(ISlice) &&
                                                                            type is { IsPublic: true, IsAbstract: false });

        // register them as singletons
        foreach (Type slice in slices)
            services.AddSingleton(typeof(ISlice), implementationType: slice);
        
        return services; 
    }
}
