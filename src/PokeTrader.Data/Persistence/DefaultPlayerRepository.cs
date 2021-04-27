using Microsoft.EntityFrameworkCore;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Trader;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeTrader.Data.Persistence
{
    public class DefaultPlayerRepository : IPlayerRepository
    {
        private readonly TradeContext _context;

        public DefaultPlayerRepository(TradeContext context)
        {
            _context = context;
        }

        public async Task Add(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }

        public async Task<Player> Get(string name)
        {
            var player = from playerDto in await _context.Players.ToArrayAsync()
                         where playerDto.Name == name
                         select playerDto.ToModel();
            return player.First();
        }

        public async Task<Player> Get(int id)
        {
            var playerDto = await _context.Players.FindAsync(id);
            return playerDto.ToModel();
        }

        public async Task<IEnumerable<Player>> Get()
        {
            var players = from playerDto in await _context.Players.ToArrayAsync()
                          select playerDto.ToModel();

            return players;
        }
    }
}
