using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

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
        public PersistentCacheBase(ILogger logger)
        {
            _asyncMutex     = new();
            _entriesByKey   = new();
            _logger         = logger;
        }

        public IEnumerable<TEntry> EnumerateEntries()
            => _entriesByKey.Values;

        public async ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
        {
            CachingLogMessages.LockAcquiring(_logger);
            var @lock = await _asyncMutex.LockAsync(cancellationToken);
            CachingLogMessages.LockAcquired(_logger);

            return @lock;
        }

        public bool RemoveEntry(TKey key)
        {
            CachingLogMessages.EntryRemoving(_logger, key);
            if (_entriesByKey.Remove(key))
            {
                CachingLogMessages.EntryRemoved(_logger, key);
                return true;
            }
            else
            {
                CachingLogMessages.EntryNotFound(_logger, key);
                return false;
            }
        }

        public void SetEntry(TEntry entry)
        {
            var key = SelectKey(entry);

            CachingLogMessages.EntryRetrieving(_logger, key);
            if (_entriesByKey.ContainsKey(key))
            {
                CachingLogMessages.EntryReplacing(_logger, key);
                _entriesByKey[SelectKey(entry)] = entry;
                CachingLogMessages.EntryReplaced(_logger, key);
            }
            else
            {
                CachingLogMessages.EntryAdding(_logger, key);
                _entriesByKey[SelectKey(entry)] = entry;
                CachingLogMessages.EntryAdded(_logger, key);
            }
        }

        public TEntry? TryGetEntry(TKey key)
        {
            CachingLogMessages.EntryRetrieving(_logger, key);
            if (_entriesByKey.TryGetValue(key, out var entry))
            {
                CachingLogMessages.EntryRetrieved(_logger, key);
                return entry;
            }
            else
            {
                CachingLogMessages.EntryNotFound(_logger, key);
                return default;
            }
        }

        protected ILogger Logger
            => _logger;

        protected override void OnDisposing(DisposalType disposalType)
        {
            if (disposalType == DisposalType.Managed)
                _asyncMutex.Dispose();

            base.OnDisposing(disposalType);
        }

        protected abstract TKey SelectKey(TEntry entry);

        private readonly AsyncMutex                 _asyncMutex;
        private readonly Dictionary<TKey, TEntry>   _entriesByKey;
        private readonly ILogger                    _logger;
    }
}
