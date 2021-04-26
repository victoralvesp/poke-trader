using PokeTrader.Core.Trader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<string>> GetNames();

        Task Add(Player player);

        Task<Player> Get(string name);

        Task<Player> Get(int id);
    }
}
