using System;
using System.Collections.Generic;
using System.Reactive.PlatformServices;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Modix.Business.Users.Tracking
{
    internal interface IUserTrackingCache
    {
        IObservable<DateTimeOffset?> OldestEntryAdded { get; }

        ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken);

        void RemoveModel(ulong userId);

        IEnumerable<UserTrackingModel> RemoveModels(TimeSpan minimumAge);

        void SetModel(UserTrackingModel model);

        UserTrackingModel? TryGetModel(ulong userId);
    }

    internal sealed class UserTrackingCache
        : IUserTrackingCache,
            IDisposable
    {
        public UserTrackingCache(ISystemClock systemClock)
        {
            _asyncMutex         = new();
            _entriesByUserId    = new();
            _entryQueue         = new();
            _oldesEntryAdded    = new(null);
            _systemClock        = systemClock;
        }

        public IObservable<DateTimeOffset?> OldestEntryAdded
            => _oldesEntryAdded;

        public void Dispose()
            => _asyncMutex.Dispose();

        public ValueTask<IDisposable> LockAsync(CancellationToken cancellationToken)
            => _asyncMutex.LockAsync(cancellationToken);

        public void RemoveModel(ulong userId)
        {
            if (_entriesByUserId.TryGetValue(userId, out var node))
            {
                _entriesByUserId.Remove(userId);
                _entryQueue.Remove(node);
            }
        }

        public IEnumerable<UserTrackingModel> RemoveModels(TimeSpan minimumAge)
        {
            var threshold = _systemClock.UtcNow - minimumAge;
            var anyRemoved = false;

            while((_entryQueue.Last is not null) && (_entryQueue.Last.Value.Added < threshold))
            {
                yield return _entryQueue.Last.Value.Model;

                _entriesByUserId.Remove(_entryQueue.Last.Value.Model.UserId);
                _entryQueue.Remove(_entryQueue.Last);

                anyRemoved = true;
            }

            if (anyRemoved)
                _oldesEntryAdded.OnNext(_entryQueue.Last?.Value.Added);
        }

        public void SetModel(UserTrackingModel model)
        {
            if (_entriesByUserId.TryGetValue(model.UserId, out var node))
            {
                var entry = node.Value;
                entry.Model = model;
                node.Value = entry;
            }
            else
            {
                node = _entryQueue.AddFirst(new CacheEntry()
                {
                    Added = _systemClock.UtcNow,
                    Model = model
                });
                _entriesByUserId.Add(model.UserId, node);

                if ((_entryQueue.Last is not null) && (_entryQueue.Count == 1))
                    _oldesEntryAdded.OnNext(_entryQueue.Last.Value.Added);
            }
        }

        public UserTrackingModel? TryGetModel(ulong userId)
            => _entriesByUserId.TryGetValue(userId, out var node)
                ? node.Value.Model
                : null;

        private readonly AsyncMutex                                     _asyncMutex;
        private readonly Dictionary<ulong, LinkedListNode<CacheEntry>>  _entriesByUserId;
        private readonly LinkedList<CacheEntry>                         _entryQueue;
        private readonly BehaviorSubject<DateTimeOffset?>               _oldesEntryAdded;
        private readonly ISystemClock                                   _systemClock;

        private struct CacheEntry
        {
            public DateTimeOffset       Added;
            public UserTrackingModel    Model;
        }
    }
}
