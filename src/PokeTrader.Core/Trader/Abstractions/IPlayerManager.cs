using PokeTrader.Core.Trader.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeTrader.Core.Trader.Abstractions
{
    public interface IPlayerManager
    {
        Task<IEnumerable<Player>> Get();

        Task<Player> Add(Player player);

        Task<Player> Get(string name);

    }
}