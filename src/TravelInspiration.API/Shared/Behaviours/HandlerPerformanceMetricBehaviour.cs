using MediatR;
using System.Diagnostics;
using TravelInspiration.API.Shared.Metrics;

namespace TravelInspiration.API.Shared.Behaviours;

public sealed class HandlerPerformanceMetricBehaviour<TRequest, TResponse>(HandlerPerformanceMetric handlerPerformanceMetric)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        TResponse response = await next(cancellationToken);
        _timer.Stop();

        handlerPerformanceMetric.MilliSecondsElapsed(_timer.ElapsedMilliseconds);

        return response;
    }
}
