using System.Threading.Tasks;

namespace System.Threading
{
    public sealed class AsyncMutex
        : IDisposable
    {
        public AsyncMutex()
            => _semaphore = new(initialCount: 1, maxCount: 1);

        public async ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            return new AsyncLock(_semaphore);
        }

        public void Dispose()
            => _semaphore.Dispose();

        private readonly SemaphoreSlim _semaphore;

        private sealed class AsyncLock
            : IDisposable
        {
            public AsyncLock(SemaphoreSlim semaphore)
                => _semaphore = semaphore;

            public void Dispose()
                => _semaphore.Release();

            private readonly SemaphoreSlim _semaphore;
        }
    }
}
