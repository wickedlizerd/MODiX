using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace Modix.Web
{
    public abstract class ViewBase<TViewModel>
        : ComponentBase,
            IDisposable
    {
        protected ViewBase()
            => _observationsBySource = new Dictionary<object, IDisposable>();

        ~ViewBase()
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
                GC.SuppressFinalize(this);
                _hasDisposed = true;
            }
        }

        [Inject]
        protected TViewModel ViewModel { get; set; }
            = default!;

        protected T ObserveValue<T>(
                    IObservable<T> source,
                    T initialValue)
                where T : notnull
            => ObserveValueInternal(source, initialValue);

        protected T? ObserveValue<T>(
                    IObservable<T?> source)
                where T : struct
            => ObserveValueInternal(source, default);

        protected T? ObserveValue<T>(
                    IObservable<T?> source)
                where T : class
            => ObserveValueInternal(source, default);

        protected virtual void OnDisposing(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                foreach (var observation in _observationsBySource.Values)
                    observation.Dispose();

                if (ViewModel is IDisposable viewModel)
                    viewModel.Dispose();
            }
        }

        private T ObserveValueInternal<T>(
            IObservable<T> source,
            T initialValue)
        {
            ValueObservation<T> observation;

            if (!_observationsBySource.TryGetValue(source, out var temp))
            {
                observation = new ValueObservation<T>(
                    source,
                    StateHasChanged,
                    initialValue);
                _observationsBySource.Add(source, observation);
            }
            else
                observation = (ValueObservation<T>)temp;

            return observation.Value;
        }

        private readonly Dictionary<object, IDisposable> _observationsBySource;

        private bool _hasDisposed;
    }
}
