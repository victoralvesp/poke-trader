using PokeTrader.Core.Trader.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeTrader.Core.Trader.Abstractions
{
    public interface IPlayerManager
    {
        Task<IEnumerable<string>> GetNames();

        Task Add(Player player);

        Task<Player> Get(string name);

    }
}