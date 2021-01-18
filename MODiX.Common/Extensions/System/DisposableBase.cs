namespace System
{
    public abstract class DisposableBase
        : IDisposable
    {
        ~DisposableBase()
        {
            if (!_hasDisposed)
            {
                OnDisposing(disposeManagedResources: false);

                _hasDisposed = true;
            }
        }

        public void Dispose()
        {
            if (!_hasDisposed)
            {
                OnDisposing(disposeManagedResources: true);

                _hasDisposed = true;

                GC.SuppressFinalize(this);
            }
        }

        protected virtual void OnDisposing(bool disposeManagedResources) { }

        private bool _hasDisposed;
    }
}
