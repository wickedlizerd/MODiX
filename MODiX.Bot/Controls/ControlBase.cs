using System;
using System.Threading;
using System.Threading.Tasks;

using Remora.Results;

namespace Modix.Bot.Controls
{
    public abstract class ControlBase
        : IDisposable
    {
        protected static async Task<ControlCreationResult<TControl>> OnCreatingAsync<TControl>(
                TControl control,
                CancellationToken cancellationToken)
            where TControl : ControlBase
        {
            try
            {
                var initializeResult = await control.InitializeAsync(cancellationToken);
                if (!initializeResult.IsSuccess)
                    return ControlCreationResult<TControl>.FromError(initializeResult);

            }
            catch (Exception ex)
            {
                control.Dispose();

                return ControlCreationResult<TControl>.FromError(ex);
            }

            return ControlCreationResult.FromControl(control);
        }

        protected ControlBase()
        {
            _resources = new();
        }

        ~ControlBase()
        {
            if (!_hasDisposed)
            {
                OnDisposing(false);
                _hasDisposed = true;
            }
        }

        public virtual Task<OperationResult> DestroyAsync()
            => Task.FromResult(OperationResult.FromSuccess());

        public void Dispose()
        {
            if (!_hasDisposed)
            {
                OnDisposing(true);
                GC.SuppressFinalize(this);
                _hasDisposed = true;
            }
        }

        protected CompositeDisposable Resources
            => _resources;

        protected virtual Task<OperationResult> InitializeAsync(CancellationToken cancellationToken)
            => Task.FromResult(OperationResult.FromSuccess());

        protected virtual void OnDisposing(bool disposeManagedResources)
        {
            if (disposeManagedResources)
                Resources.Dispose();
        }

        private readonly CompositeDisposable _resources;

        private bool _hasDisposed;
    }
}
