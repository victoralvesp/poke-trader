using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IHistoryRepository<T>
    where T : notnull
    {
        Task<IEnumerable<T>> GetAsync();
        Task AddAsync(T item);

        IEnumerable<T> Get();

        void Add(T item);
    }
}