using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Modix.Business.Caching
{
    public interface IPersistentCache<TKey, TEntry>
        where TKey : notnull
        where TEntry : notnull
    {
        IEnumerable<TEntry> EnumerateEntries();

        ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken);

        bool RemoveEntry(TKey key);

        void SetEntry(TEntry entry);

        TEntry? TryGetEntry(TKey key);
    }

    public abstract class PersistentCacheBase<TKey, TEntry>
            : DisposableBase,
                IPersistentCache<TKey, TEntry>
        where TKey : notnull
        where TEntry : notnull
    {
        public PersistentCacheBase()
        {
            _asyncMutex     = new();
            _entriesByKey   = new();
        }

        public IEnumerable<TEntry> EnumerateEntries()
            => _entriesByKey.Values;

        public ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
            => _asyncMutex.LockAsync(cancellationToken);

        public bool RemoveEntry(TKey key)
            => _entriesByKey.Remove(key);

        public void SetEntry(TEntry entry)
            => _entriesByKey[SelectKey(entry)] = entry;

        public TEntry? TryGetEntry(TKey key)
            => _entriesByKey.TryGetValue(key, out var entry)
                ? entry
                : default;

        protected override void OnDisposing(DisposalType disposalType)
        {
            if (disposalType == DisposalType.Managed)
                _asyncMutex.Dispose();

            base.OnDisposing(disposalType);
        }

        protected abstract TKey SelectKey(TEntry entry);

        private readonly AsyncMutex                 _asyncMutex;
        private readonly Dictionary<TKey, TEntry>   _entriesByKey;
    }
}
