using System;
using System.Reactive.Linq;

namespace Modix.Web
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {
            Now = Observable.Timer(
                    dueTime:    TimeSpan.Zero,
                    period:     TimeSpan.FromMilliseconds(100))
                .Select(_ => DateTime.Now.TruncateMilliseconds())
                .DistinctUntilChanged();
        }

        public IObservable<DateTime> Now { get; }
    }
}
