using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Itineraries;

public sealed class GetItineraries : ISlice
{ 
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(pattern: "api/itineraries", (string? searchFor, 
                                                                 IMediator mediator,
                                                                 CancellationToken cancellationToken) => mediator.Send(new GetItinerariesQuery(searchFor), cancellationToken));
    }

    public sealed class GetItinerariesQuery(string? searchFor) : IRequest<IResult>
    {
        public string? SearchFor { get; } = searchFor;
    } 

    public sealed class GetItinerariesHandler(TravelInspirationDbContext dbContext, IMapper mapper) : IRequestHandler<GetItinerariesQuery, IResult>
    {
        public async Task<IResult> Handle(GetItinerariesQuery request, CancellationToken cancellationToken)
        {
            return Results.Ok(mapper.Map<IEnumerable<ItineraryDto>>(await dbContext.Itineraries.Where(itinerary =>
                                                                    request.SearchFor == null ||
                                                                    itinerary.Name.Contains(request.SearchFor) ||
                                                                    (itinerary.Description != null && itinerary.Description.Contains(request.SearchFor))).ToListAsync(cancellationToken))); 
        }
    }

    public sealed class ItineraryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string UserId { get; set; }
    }

    public sealed class ItineraryMapProfile : Profile
    {
        public ItineraryMapProfile()
        {
            CreateMap<Itinerary, ItineraryDto>();
        }
    }
}
