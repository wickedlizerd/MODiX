namespace System.Reactive.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> Share<T>(this IObservable<T> source)
            => source
                .Publish()
                .RefCount();
    }
}
