using System;

namespace Modix.Web
{
    public class ViewModelBase
        : IDisposable
    {
        ~ViewModelBase()
        {
            if (!_hasDisposed)
            {
                OnDisposing(disposeManagedResources: false);
            }
        }

        public void Dispose()
        {
            if (!_hasDisposed)
            {
                OnDisposing(disposeManagedResources: true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void OnDisposing(bool disposeManagedResources)
        {
            _hasDisposed = true;
        }

        private bool _hasDisposed;
    }
}
