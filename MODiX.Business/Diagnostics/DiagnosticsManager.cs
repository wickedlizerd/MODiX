using System;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;

namespace Modix.Business.Diagnostics
{
    public interface IDiagnosticsManager
    {
        IObservable<DateTimeOffset> Now { get; }
    }

    public class DiagnosticsManager
        : IDiagnosticsManager
    {
        public DiagnosticsManager(ISystemClock systemClock)
            => Now = Observable.Timer(
                    dueTime:    TimeSpan.Zero,
                    period:     TimeSpan.FromMilliseconds(10))
                .Select(_ => systemClock.UtcNow
                    .ToLocalTime()
                    .TruncateMilliseconds())
                .DistinctUntilChanged()
                .ShareReplay(1);

        public IObservable<DateTimeOffset> Now { get; }
    }
}
