using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class CreateStop : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(pattern: "api/itineraries/{itineraryId}/stops", (int itineraryId,
                                                                                    CreateStopCommand createStopCommand,
                                                                                    IMediator mediator, 
                                                                                    CancellationToken cancellationToken) =>
        {
             createStopCommand.ItineraryId = itineraryId;
             return mediator.Send(createStopCommand, cancellationToken);
        });
    }

    public sealed class CreateStopCommand(int itineraryId, string name, string? imageUri) : IRequest<IResult>
    {
        public int ItineraryId { get; set; } = itineraryId;
        public string Name { get; } = name;
        public string? ImageUri { get; } = imageUri;
    }

    public sealed class CreateStopCommandValidator : AbstractValidator<CreateStopCommand>
    {
        public CreateStopCommandValidator()
        {
            RuleFor(createStopCommand => createStopCommand.Name).MaximumLength(200).NotEmpty();
            
            RuleFor(createStopCommand => createStopCommand.ImageUri).Must(imageUri => Uri.TryCreate(imageUri ?? "", UriKind.Absolute, out _))
                                                                    .When(createStopCommand => !string.IsNullOrEmpty(createStopCommand.ImageUri))
                                                                    .WithMessage("ImageUri must be a valid Uri.");
        }
    }

    public sealed class CreateStopCommandHandler(TravelInspirationDbContext dbContext, IMapper mapper) : IRequestHandler<CreateStopCommand, IResult>
    {
        public async Task<IResult> Handle(CreateStopCommand request, CancellationToken cancellationToken)
        {
            // check if itinerary exists 
            if (!await dbContext.Itineraries.AnyAsync(i => i.Id == request.ItineraryId, cancellationToken))
                return Results.NotFound();

            // create the entity
            var stopEntity = new Stop(request.Name);
            stopEntity.HandleCreateCommand(request);

            dbContext.Stops.Add(stopEntity);
            await dbContext.SaveChangesAsync(cancellationToken);

            // map to DTO before returning it 
            return Results.Created(uri: $"api/itineraries/{stopEntity.ItineraryId}/stops/{stopEntity.Id}", mapper.Map<StopDto>(stopEntity));
        }
    }

    public sealed class StopDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; } 
        public Uri? ImageUri { get; set; }
        public required int ItineraryId { get; set; }
    }

    public sealed class StopMapProfileAfterCreation : Profile
    {
        public StopMapProfileAfterCreation()
        {
            CreateMap<Stop, StopDto>();
        }
    }
}
