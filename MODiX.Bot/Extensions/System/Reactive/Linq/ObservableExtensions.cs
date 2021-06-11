using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> DoOnError<T>(this IObservable<T> source, Action<Exception> onError)
            => Observable.Create<T>(observer => source.Subscribe(
                observer.OnNext,
                ex =>
                {
                    onError.Invoke(ex);
                    observer.OnError(ex);
                },
                observer.OnCompleted));

        public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<T, Task> selector)
            => source.SelectMany(value => Observable.FromAsync(() => selector.Invoke(value)));

        public static IObservable<T> Throw<T>(this IObservable<Exception> source)
            => source.Select<Exception, T>(ex => throw ex);
    }
}
