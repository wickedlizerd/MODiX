using System;
using System.Collections.Generic;
using System.Reactive.PlatformServices;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

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
        protected FifoCacheBase(ISystemClock systemClock)
        {
            _asyncMutex         = new();
            _entriesByKey       = new();
            _entryQueue         = new();
            _oldestEntryAdded   = new(null);
            _systemClock        = systemClock;
        }

        public IObservable<DateTimeOffset?> OldestEntryAdded
            => _oldestEntryAdded;

        public ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
            => _asyncMutex.LockAsync(cancellationToken);

        public bool RemoveEntry(TKey key)
        {
            if (_entriesByKey.TryGetValue(key, out var node))
            {
                _entriesByKey.Remove(key);
                _entryQueue.Remove(node);

                return true;
            }

            return false;
        }

        public IEnumerable<TEntry> RemoveOldEntries(TimeSpan minimumAge)
        {
            var threshold = _systemClock.UtcNow - minimumAge;
            var anyRemoved = false;

            while((_entryQueue.Last is not null) && (_entryQueue.Last.Value.Added <= threshold))
            {
                yield return _entryQueue.Last.Value.Entry;

                _entriesByKey.Remove(SelectKey(_entryQueue.Last.Value.Entry));
                _entryQueue.Remove(_entryQueue.Last);

                anyRemoved = true;
            }

            if (anyRemoved)
                _oldestEntryAdded.OnNext(_entryQueue.Last?.Value.Added);
        }

        public void SetEntry(TEntry entry)
        {
            if (_entriesByKey.TryGetValue(SelectKey(entry), out var node))
            {
                var nodeValue = node.Value;
                nodeValue.Entry = entry;
                node.Value = nodeValue;
            }
            else
            {
                node = _entryQueue.AddFirst(new NodeValue()
                {
                    Added = _systemClock.UtcNow,
                    Entry = entry
                });
                _entriesByKey.Add(SelectKey(entry), node);

                if ((_entryQueue.Last is not null) && (_entryQueue.Count == 1))
                    _oldestEntryAdded.OnNext(_entryQueue.Last.Value.Added);
            }
        }

        public TEntry? TryGetEntry(TKey userId)
            => _entriesByKey.TryGetValue(userId, out var node)
                ? node.Value.Entry
                : default;

        protected override void OnDisposing(DisposalType disposalType)
        {
            if (disposalType == DisposalType.Managed)
                _asyncMutex.Dispose();

            base.OnDisposing(disposalType);
        }

        protected abstract TKey SelectKey(TEntry entry);

        private readonly AsyncMutex                                     _asyncMutex;
        private readonly Dictionary<TKey, LinkedListNode<NodeValue>>    _entriesByKey;
        private readonly LinkedList<NodeValue>                          _entryQueue;
        private readonly BehaviorSubject<DateTimeOffset?>               _oldestEntryAdded;
        private readonly ISystemClock                                   _systemClock;

        private struct NodeValue
        {
            public DateTimeOffset   Added;
            public TEntry           Entry;
        }
    }
}
