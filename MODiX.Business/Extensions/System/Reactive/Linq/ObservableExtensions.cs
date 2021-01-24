namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> WhereNotNull<T>(this IObservable<T?> source)
                where T : class
            => source.Where(value => value is not null)!;
    }
}
