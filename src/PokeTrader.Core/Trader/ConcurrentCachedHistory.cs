using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Abstractions;
using System.Reactive.Subjects;

namespace PokeTrader.Core.Trader
{
    public class ConcurrentCachedHistory<T> : IHistory<T>, IDisposable
    where T : notnull
    {
        ConcurrentBag<T> _cache = new();
        IHistoryRepository<T> _repo;
        private bool _disposed = false;
        internal static readonly TimeSpan _cacheTime = TimeSpan.FromSeconds(.3);
        readonly Subject<bool> _syncSubject = new();

        public ConcurrentCachedHistory(IHistoryRepository<T> repo)
        {
            _repo = repo;
            InitializeSyncCycle();
        }

        private void InitializeSyncCycle()
        {
            _ = Task.Run(SyncCycle);
        }

        private async void SyncCycle()
        {
            while(!_disposed)
            {
                var persistedHistory = await _repo.Get();
                var notPersisted = _cache.Where(t => !persistedHistory.Any(u => u.Equals(t))).ToArray();
                var notCached = persistedHistory.Where(t => !_cache.Any(u => u.Equals(t))).ToArray();
                if (notPersisted.Any() || notCached.Any())
                {
                    _syncSubject.OnNext(false);

                    await _repo.Add(notPersisted);
                    _cache = new(_cache.ToArray().Concat(notCached).ToArray());
                }
                _syncSubject.OnNext(true);
                await Task.Delay(_cacheTime);
            }
        }

        public void Add(T target)
        {
            _cache.Add(target);
        }

        public IEnumerable<T> Get()
        {
            return _cache.ToArray();
        }

        private async Task EnsureSynchronized()
        {
            var synched = false;
            _syncSubject.Subscribe((sync) => synched = sync);
            while(!synched)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
        }

        public IEnumerable<T> Get(IFilter<T> trade)
        {
            return _cache.ToArray()
                         .Where(trade.Pass);
        }

        public void Dispose()
        {
            _disposed = true;
            _syncSubject.Dispose();
        }
    }
}