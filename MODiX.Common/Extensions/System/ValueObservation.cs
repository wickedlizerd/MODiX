namespace System
{
    public sealed class ValueObservation<T>
        : IDisposable
    {
        public ValueObservation(
            IObservable<T> source,
            Action onValueChanged,
            T initialValue)
        {
            _value = initialValue;

            _subscription = source.Subscribe(value =>
            {
                _value = value;
                onValueChanged.Invoke();
            });
        }

        public T Value
            => _value;

        public void Dispose()
        {
            if (!_hasDisposed)
            {
                _subscription.Dispose();
                _hasDisposed = true;
            }
        }

        private bool _hasDisposed;
        private IDisposable _subscription;
        private T _value;
    }
}
