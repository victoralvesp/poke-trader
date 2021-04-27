using System.Collections.Generic;
using System.Threading.Tasks;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Abstractions;
using PokeTrader.Core.Trader.Models;

namespace PokeTrader.Core.Trader
{
    public class DefaultPlayerManager : IPlayerManager
    {
        readonly IPlayerRepository _repo;

        public DefaultPlayerManager(IPlayerRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Player>> Get()
        => _repo.Get();

        public async Task<Player> Add(Player player)
        {
            // validate if required and check if any data population is needed
            await _repo.Add(player);
            return player;
        }

        public Task<Player> Get(string name)
        => _repo.Get(name);

    }
}