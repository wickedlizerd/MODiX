namespace System
{
    public sealed class DelegateDisposable
        : IDisposable
    {
        public static DelegateDisposable Create(Action<DelegateDisposable> onDisposing)
            => new DelegateDisposable(onDisposing);

        public DelegateDisposable(Action<DelegateDisposable> onDisposing)
        {
            _onDisposing = onDisposing;
        }

        public void Dispose()
        {
            if (!_hasDisposaed)
            {
                _onDisposing.Invoke(this);
                _hasDisposaed = true;
            }
        }

        private bool _hasDisposaed;
        private Action<DelegateDisposable> _onDisposing;
    }
}
