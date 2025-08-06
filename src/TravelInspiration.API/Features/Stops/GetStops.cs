using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Features.Stops;

public static class GetStops
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(pattern: "api/itineraries/{itineraryId}/stops", async (int itineraryId,
                                                                          ILoggerFactory logger,
                                                                          TravelInspirationDbContext dbContext,
                                                                          IMapper mapper,
                                                                          CancellationToken cancellationToken) =>
            {
                logger.CreateLogger(categoryName: "EndpointHandlers").LogInformation("GetStops feature called.");

                Itinerary? itinerary = await dbContext.Itineraries.Include(navigationPropertyPath: itinerary => itinerary.Stops)
                                                                  .FirstOrDefaultAsync(itinerary => itinerary.Id == itineraryId, cancellationToken);

                return itinerary == null ? Results.NotFound() : Results.Ok(mapper.Map<IEnumerable<StopDto>>(itinerary.Stops));
            });
    }

    public sealed class StopDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public Uri? ImageUri { get; set; }
        public required int ItineraryId { get; set; }
    }

    public sealed class StopMapProfile : Profile
    {
        public StopMapProfile()
        {
            CreateMap<Stop, StopDto>();    
        }
    }
}
