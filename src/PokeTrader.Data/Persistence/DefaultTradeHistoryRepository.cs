using Microsoft.EntityFrameworkCore;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Extensions;
using PokeTrader.Dto.Trader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrader.Data.Persistence
{

    public class DefaultTradeHistoryRepository : IHistoryRepository<Trade>
    {
        private readonly TradeContext _context;

        public DefaultTradeHistoryRepository(TradeContext context)
        {
            _context = context;
        }

        public async Task Add(params Trade[] items)
        {
            await _context.Trades.AddRangeAsync(items.Select(item => (TradeDto)item).ToArray());
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Trade>> Get() => (await _context.Trades.ToArrayAsync()).ToModel();
    }
}
