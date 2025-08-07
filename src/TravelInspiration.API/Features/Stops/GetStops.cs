using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class GetStops : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(pattern: "api/itineraries/{itineraryId}/stops", (int itineraryId,
                                                                                     ILoggerFactory logger, 
                                                                                     IMediator mediator,
                                                                                     CancellationToken cancellationToken) =>
            {
                logger.CreateLogger(categoryName: "EndpointHandlers").LogInformation("GetStops feature called.");

                 return mediator.Send(new GetStopsQuery(itineraryId), cancellationToken); 
            });
    }

    public sealed class GetStopsQuery(int itineraryId) : IRequest<IResult>
    {
        public int ItineraryId { get; } = itineraryId;
    }

    public sealed class GetStopsHandler(TravelInspirationDbContext dbContext, IMapper mapper) : IRequestHandler<GetStopsQuery, IResult>
    {
        public async Task<IResult> Handle(GetStopsQuery request, CancellationToken cancellationToken)
        {
            Itinerary? itinerary = await dbContext.Itineraries.Include(navigationPropertyPath: itinerary => itinerary.Stops)
                                                              .FirstOrDefaultAsync(itinerary => itinerary.Id == request.ItineraryId, cancellationToken);

            return itinerary == null ? Results.NotFound() : Results.Ok(mapper.Map<IEnumerable<StopDto>>(itinerary.Stops));
        }
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
