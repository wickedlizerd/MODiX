using System;
using System.Collections.Generic;
using System.Reactive.PlatformServices;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Modix.Business.Caching
{
    public interface IFifoCache<TKey, TEntry>
        where TKey : notnull
        where TEntry : notnull
    {
        IObservable<DateTimeOffset?> OldestEntryAdded { get; }

        ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken);

        bool RemoveEntry(TKey key);

        IEnumerable<TEntry> RemoveOldEntries(TimeSpan minimumAge);

        void SetEntry(TEntry entry);

        TEntry? TryGetEntry(TKey key);
    }

    public abstract class FifoCacheBase<TKey, TEntry>
            : DisposableBase,
                IFifoCache<TKey, TEntry>
        where TKey : notnull
        where TEntry : notnull
    {
        protected FifoCacheBase(
            ILogger         logger,
            ISystemClock    systemClock)
        {
            _asyncMutex         = new();
            _entriesByKey       = new();
            _entryQueue         = new();
            _logger             = logger;
            _oldestEntryAdded   = new(null);
            _systemClock        = systemClock;
        }

        public IObservable<DateTimeOffset?> OldestEntryAdded
            => _oldestEntryAdded;

        public async ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
        {
            CachingLogMessages.LockAcquiring(_logger);
            var @lock = await _asyncMutex.LockAsync(cancellationToken);
            CachingLogMessages.LockAcquired(_logger);

            return @lock;
        }

        public bool RemoveEntry(TKey key)
        {
            CachingLogMessages.EntryRetrieving(_logger, key);
            if (_entriesByKey.TryGetValue(key, out var node))
            {
                CachingLogMessages.EntryRemoving(_logger, key, node.Value.Added);

                _entriesByKey.Remove(key);
                _entryQueue.Remove(node);

                var oldestEntryAdded = _entryQueue.Last?.Value.Added;
                if (oldestEntryAdded != _oldestEntryAdded.Value)
                    ChangeOldestEntryAdded(oldestEntryAdded);

                CachingLogMessages.EntryRemoved(_logger, key, node.Value.Added);
                return true;
            }

            CachingLogMessages.EntryNotFound(_logger, key);
            return false;
        }

        public IEnumerable<TEntry> RemoveOldEntries(TimeSpan minimumAge)
        {
            var threshold = _systemClock.UtcNow - minimumAge;
            var anyRemoved = false;

            CachingLogMessages.OldEntriesRemoving(_logger, minimumAge);
            while ((_entryQueue.Last is not null) && (_entryQueue.Last.Value.Added <= threshold))
            {
                var nodeToRemove = _entryQueue.Last;
                var key = SelectKey(nodeToRemove.Value.Entry);
                CachingLogMessages.EntryRemoving(_logger, key, nodeToRemove.Value.Added);

                yield return nodeToRemove.Value.Entry;

                _entriesByKey.Remove(SelectKey(_entryQueue.Last.Value.Entry));
                _entryQueue.Remove(_entryQueue.Last);

                anyRemoved = true;

                CachingLogMessages.EntryRemoved(_logger, key, nodeToRemove.Value.Added);
            }
            CachingLogMessages.OldEntriesRemoved(_logger);

            if (anyRemoved)
                ChangeOldestEntryAdded(_entryQueue.Last?.Value.Added);
        }

        public void SetEntry(TEntry entry)
        {
            var key = SelectKey(entry);

            CachingLogMessages.EntryRetrieving(_logger, key);
            if (_entriesByKey.TryGetValue(key, out var node))
            {
                CachingLogMessages.EntryReplacing(_logger, key, node.Value.Added);

                var nodeValue = node.Value;
                nodeValue.Entry = entry;
                node.Value = nodeValue;

                CachingLogMessages.EntryReplaced(_logger, key, node.Value.Added);
            }
            else
            {
                CachingLogMessages.EntryAdding(_logger, key);

                node = _entryQueue.AddFirst(new NodeValue()
                {
                    Added = _systemClock.UtcNow,
                    Entry = entry
                });
                _entriesByKey.Add(SelectKey(entry), node);

                CachingLogMessages.EntryAdded(_logger, key, node.Value.Added);

                if ((_entryQueue.Last is not null) && (_entryQueue.Count == 1))
                    ChangeOldestEntryAdded(_entryQueue.Last.Value.Added);
            }
        }

        public TEntry? TryGetEntry(TKey key)
        {
            CachingLogMessages.EntryRetrieving(_logger, key);
            if (_entriesByKey.TryGetValue(key, out var node))
            {
                CachingLogMessages.EntryRetrieved(_logger, key, node.Value.Added);
                return node.Value.Entry;
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

        private void ChangeOldestEntryAdded(DateTimeOffset? value)
        {
            CachingLogMessages.OldestEntryChanging(_logger, value);
            _oldestEntryAdded.OnNext(value);
            CachingLogMessages.OldestEntryChanged(_logger, value);
        }

        private readonly AsyncMutex                                     _asyncMutex;
        private readonly Dictionary<TKey, LinkedListNode<NodeValue>>    _entriesByKey;
        private readonly LinkedList<NodeValue>                          _entryQueue;
        private readonly ILogger                                        _logger;
        private readonly BehaviorSubject<DateTimeOffset?>               _oldestEntryAdded;
        private readonly ISystemClock                                   _systemClock;

        private struct NodeValue
        {
            public DateTimeOffset   Added;
            public TEntry           Entry;
        }
    }
}
