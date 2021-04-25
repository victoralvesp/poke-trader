using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IHistoryRepository<T>
    where T : notnull
    {
        Task<IEnumerable<T>> Get();
        Task Add(params T[] item);
    }
}