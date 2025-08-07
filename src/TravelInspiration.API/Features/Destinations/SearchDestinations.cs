using TravelInspiration.API.Shared.Domain.Models;
using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Destinations;

public sealed class SearchDestinations : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(pattern: "api/destinations", async (string? searchFor,
                                                                        ILoggerFactory logger,
                                                                        CancellationToken cancellationToken,
                                                                        IDestinationSearchApiClient destinationSearchApiClient) =>
        {
            logger.CreateLogger(categoryName: "EndpointHandlers").LogInformation("SearchDestinations feature called.");

            IEnumerable<Destination> resultFromApiCall = await destinationSearchApiClient.GetDestinationsAsync(searchFor, cancellationToken); 

            // project the result
            var result = resultFromApiCall.Select(destination => new
            {                
                destination.Name,
                destination.Description,
                destination.ImageUri                
            });

            return Results.Ok(result);
        });
    }
}
