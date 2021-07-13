namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> WhereNotNull<T>(this IObservable<T?> source)
                where T : class
            => Observable.Create<T>(observer => source.Subscribe(
                onNext:         value =>
                {
                    if (value is not null)
                        observer.OnNext(value);
                },
                onError:        observer.OnError,
                onCompleted:    observer.OnCompleted));
    }
}
