using System.Collections.ObjectModel;

namespace System
{
    public sealed class CompositeDisposable
        : Collection<IDisposable>,
            IDisposable
    {
        public void Dispose()
        {
            if (!_hasDisposed)
            {
                foreach (var disposable in this)
                    disposable.Dispose();

                _hasDisposed = true;
            }
        }

        private bool _hasDisposed;
    }
}
