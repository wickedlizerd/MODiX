namespace System
{
    public class DisposableBase
        : IDisposable
    {
        ~DisposableBase()
        {
            if (!_hasDisposed)
            {
                OnDisposing(DisposalType.Unmanaged);
                _hasDisposed = true;
            }
        }

        public void Dispose()
        {
            if (!_hasDisposed)
            {
                OnDisposing(DisposalType.Managed);
                GC.SuppressFinalize(this);
                _hasDisposed = true;
            }
        }

        protected virtual void OnDisposing(DisposalType disposalType) { }

        private bool _hasDisposed;
    }
}
