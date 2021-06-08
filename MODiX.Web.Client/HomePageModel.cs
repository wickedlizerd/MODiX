using System;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;

namespace Modix.Web.Client
{
    public class HomePageModel
    {
        public HomePageModel(ISystemClock systemClock)
            => Now = Observable.Timer(
                    dueTime:    TimeSpan.Zero,
                    period:     TimeSpan.FromMilliseconds(10))
                .Select(_ => systemClock.UtcNow
                    .ToLocalTime()
                    .TruncateMilliseconds())
                .DistinctUntilChanged();

        public IObservable<DateTimeOffset> Now { get; }
    }
}
