using System;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;

using Microsoft.Extensions.Logging;

namespace Modix.Business.Diagnostics
{
    public interface IDiagnosticsManager
    {
        IObservable<DateTimeOffset> SystemClock { get; }
    }

    public class DiagnosticsManager
        : IDiagnosticsManager
    {
        public DiagnosticsManager(
                ILogger<DiagnosticsManager> logger,
                ISystemClock                systemClock)
            => SystemClock = Observable.Timer(
                    dueTime:    TimeSpan.Zero,
                    period:     TimeSpan.FromMilliseconds(10))
                .OnSubscribing(() => DiagnosticsLogMessages.SystemClockStarting(logger))
                .Select(_ => systemClock.UtcNow
                    .ToLocalTime()
                    .TruncateMilliseconds())
                .DistinctUntilChanged()
                .Finally(() => DiagnosticsLogMessages.SystemClockStopping(logger))
                .ShareReplay(1);

        public IObservable<DateTimeOffset> SystemClock { get; }
    }
}
