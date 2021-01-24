using System.Threading.Tasks;

namespace System
{
    public class AsyncDisposableBase
        : IAsyncDisposable
    {
        ~AsyncDisposableBase()
            => OnFinalizingAsync();

        public async ValueTask DisposeAsync()
        {
            if (!_hasDisposed)
            {
                await OnDisposingAsync(DisposalType.Managed);
                #pragma warning disable CA1816 // https://github.com/dotnet/roslyn-analyzers/issues/3675
                GC.SuppressFinalize(this);
                #pragma warning restore CA1816
                _hasDisposed = true;
            }
        }

        protected virtual ValueTask OnDisposingAsync(DisposalType type)
            => ValueTask.CompletedTask;

        private async void OnFinalizingAsync()
        {
            if (!_hasDisposed)
            {
                await OnDisposingAsync(DisposalType.Unmanaged);
                _hasDisposed = true;
            }
        }            

        private bool _hasDisposed;
    }
}
