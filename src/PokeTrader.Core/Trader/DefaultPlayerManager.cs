using System.Collections.Generic;
using System.Threading.Tasks;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Core.Trader
{
    public class DefaultPlayerManager : IPlayerManager
    {
        IPlayerRepository _repo;

        public DefaultPlayerManager(IPlayerRepository repo)
        {
            _repo = repo;
        }

        Task<IEnumerable<string>> IPlayerManager.GetNames()
        => _repo.GetNames();

        public Task Add(Player player)
        => _repo.Add(player);

        Task<Player> IPlayerManager.Get(string name)
        => _repo.Get(name);

    }
}