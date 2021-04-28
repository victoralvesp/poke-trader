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
    public class DefaultHistory<T> : IHistory<T>
    where T : notnull
    {
        readonly IHistoryRepository<T> _repo;

        public DefaultHistory(IHistoryRepository<T> repo)
        {
            _repo = repo;
        }

        public void Add(T target)
        {
            _repo.Add(target);
        }

        public IEnumerable<T> Get()
        {
            return _repo.Get();
        }

        public IEnumerable<T> Get(IFilter<T> filter)
        {
            return _repo.Get()
                         .Where(filter.Pass);
        }

    }
}