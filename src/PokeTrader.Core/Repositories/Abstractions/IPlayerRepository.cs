using PokeTrader.Core.Trader.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> Get();

        Task Add(Player player);

        Task<Player> Get(string name);

        Task<Player> Get(int id);
    }
}
