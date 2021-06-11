namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> WhereNotNull<T>(this IObservable<T?> source)
                where T : struct
            => Observable.Create<T>(observer => source.Subscribe(
                onNext:         next =>
                {
                    if (next.HasValue)
                        observer.OnNext(next.Value);
                },
                onError:        observer.OnError,
                onCompleted:    observer.OnCompleted));
    }
}
