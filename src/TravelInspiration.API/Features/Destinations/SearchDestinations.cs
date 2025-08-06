using TravelInspiration.API.Shared.Domain.Models;
using TravelInspiration.API.Shared.Networking;

namespace TravelInspiration.API.Features.Destinations;

public static class SearchDestinations
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "api/destinations", async (string? searchFor,
                                                       ILoggerFactory logger,
                                                       CancellationToken cancellationToken,
                                                       IDestinationSearchApiClient destinationSearchApiClient) =>
        {
            logger.CreateLogger(categoryName: "EndpointHandlers").LogInformation("SearchDestinations feature called.");

            IEnumerable<Destination> resultFromApiCall = await destinationSearchApiClient.GetDestinationsAsync(searchFor, cancellationToken); 

            // project the result
            var result = resultFromApiCall.Select(d => new
            {                
                d.Name,
                d.Description,
                d.ImageUri                
            });

            return Results.Ok(result);
        });
    }
}
