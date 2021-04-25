using System.Collections.Generic;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IHistoryRepository<T>
    where T : notnull
    {
        IEnumerable<T> Get();
    }
}