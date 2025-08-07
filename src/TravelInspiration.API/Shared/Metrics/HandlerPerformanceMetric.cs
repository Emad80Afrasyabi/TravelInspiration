using System.Diagnostics.Metrics;

namespace TravelInspiration.API.Shared.Metrics;

public sealed class HandlerPerformanceMetric
{
    private readonly Counter<long> _milliSecondsElapsed;
    public HandlerPerformanceMetric(IMeterFactory meterFactory)
    {
        // a meter
        Meter meter = meterFactory.Create(name: "TravelInspiration.API");
        
        _milliSecondsElapsed = meter.CreateCounter<long>(name: "travelinspiration.api.requesthandler.millisecondselapsed");        
    }

    public void MilliSecondsElapsed(long milliSecondsElapsed) => _milliSecondsElapsed.Add(milliSecondsElapsed);
}
