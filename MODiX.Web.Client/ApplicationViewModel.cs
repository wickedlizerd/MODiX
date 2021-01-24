using System;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;

namespace Modix.Web
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel(ISystemClock systemClock)
            => Now = Observable.Timer(
                    dueTime:    TimeSpan.Zero,
                    period:     TimeSpan.FromMilliseconds(100))
                .Select(_ => systemClock.UtcNow
                    .TruncateMilliseconds()
                    .ToLocalTime())
                .DistinctUntilChanged();

        public IObservable<DateTimeOffset> Now { get; }
    }
}
