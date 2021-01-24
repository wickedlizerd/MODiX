using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<T> WithSilentCancellation<T>(this IAsyncEnumerable<T> source)
        {
            // https://github.com/dotnet/reactive/issues/1472 Can maybe someday replace this entire method with a call to `.Catch()`
            return Core();

            async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                var whenCancelledSource = new TaskCompletionSource();

                cancellationToken.Register(whenCancelledSource.SetResult);

                await using var enumerator = source.GetAsyncEnumerator(CancellationToken.None);
                while (!cancellationToken.IsCancellationRequested)
                {
                    var whenMoved = enumerator.MoveNextAsync();

                    await Task.WhenAny(whenMoved.AsTask(), whenCancelledSource.Task);

                    if (whenMoved.IsCompletedSuccessfully && whenMoved.Result)
                        yield return enumerator.Current;
                }
            }
        }
    }
}
