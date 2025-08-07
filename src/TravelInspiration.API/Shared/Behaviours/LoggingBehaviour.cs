using MediatR.Pipeline;

namespace TravelInspiration.API.Shared.Behaviours;

public sealed class LoggingBehaviour<TRequest>(ILogger<TRequest> logger) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting feature execution: {featureFromRequestName}", typeof(TRequest).Name);
        
        return Task.CompletedTask;
    }
}
