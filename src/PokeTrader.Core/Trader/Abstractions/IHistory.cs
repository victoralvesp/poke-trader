using System.Collections.Generic;
using PokeTrader.Core.Filters.Abstractions;

namespace PokeTrader.Core.Trader.Abstractions
{
    public interface IHistory<T> where T : notnull
    {
        void Add(T target);

        IEnumerable<T> Get();
        IEnumerable<T> Get(IFilter<T> trade);
    }

}