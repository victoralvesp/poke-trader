using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Abstractions;

namespace PokeTrader.Core.Trader
{
    public class ConcurrentCachedHistory<T> : IHistory<T>
    where T : notnull
    {
        ConcurrentBag<T> _history = new();
        private bool _inSync;
        IHistoryRepository<T> _repo;

        public ConcurrentCachedHistory(IHistoryRepository<T> repo)
        {
            _repo = repo;
        }

        public void Add(T target)
        {
            _history.Add(target);
        }

        public IEnumerable<T> Get()
        {
            if(!_inSync)
            {
                SynchronizeWithRepo();
            }
            return _history.ToArray();
        }

        private void SynchronizeWithRepo()
        {
            var repoHistory = _repo.Get();
            _history = new ConcurrentBag<T>(repoHistory.Concat(_history.ToArray()));
            _inSync = true;
        }

        public IEnumerable<T> Get(IFilter<T> trade)
        {
            if(!_inSync)
            {
                SynchronizeWithRepo();
            }
            return _history.ToArray()
                           .Where(trade.Pass);
        }
    }
}