using Microsoft.EntityFrameworkCore;
using MoreLinq;
using PokeTrader.Core.Repositories.Abstractions;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Extensions;
using PokeTrader.Dto.Pokemons;
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

        public void Add(Trade item)
        {
            var dto = (TradeDto)item;
            //var pokemonsTracked = addOrAttachPokemon(dto);
            dto.Info.First.Trader = _context.Players.Attach(item.Info.First.Trader).Entity;
            dto.Info.Second.Trader = _context.Players.Attach(item.Info.Second.Trader).Entity;
            //dto.Info.First.TradeOffers = getFromTracked(pokemonsTracked, dto.Info.First.TradeOffers).ToList();
            //dto.Info.Second.TradeOffers = getFromTracked(pokemonsTracked, dto.Info.Second.TradeOffers).ToList();
            _context.TradeParticipants.Add(dto.Info.First);
            _context.TradeParticipants.Add(dto.Info.Second);
            _context.TradeInfos.Add(dto.Info);
            _context.Trades.Add(dto);
            _context.SaveChanges();

            IEnumerable<PokemonDto> addOrAttachPokemon(TradeDto item)
            {
                var fTradeOffers = item.Info.First.TradeOffers.Select(to => to);
                var sTradeOffers = item.Info.Second.TradeOffers.Select(to => to);
                var pokemons = fTradeOffers.Concat(sTradeOffers).DistinctBy(pk => pk.Id);
                var pokemonsToAdd = from pokemon in pokemons
                                    where !_context.Pokemons.Any(pk => pk.Id == pokemon.Id)
                                    select pokemon;
                var pokemonsToAttach = from pokemon in pokemons
                                       where _context.Pokemons.Any(pk => pk.Id == pokemon.Id)
                                       select pokemon;
                _context.AddRange(pokemonsToAdd);
                _context.AttachRange(pokemonsToAttach);
                return pokemonsToAdd.Concat(pokemonsToAttach);
            }
            IEnumerable<PokemonDto> getFromTracked(IEnumerable<PokemonDto> tracked, IEnumerable<PokemonDto> original)
            {
                return from ori in original
                       select tracked.First(pk => pk.Id == ori.Id);
            }
        }

        public async Task AddAsync(Trade item)
        {
            _context.Entry((PlayerDto)item.Info.First.Trader).State = EntityState.Unchanged;
            _context.Entry((PlayerDto)item.Info.Second.Trader).State = EntityState.Unchanged;
            await _context.Trades.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Trade> Get()
        {
            return (_context.Trades.Include(t => t.Info)
                                   .ThenInclude(ti => ti.First)
                                   .ThenInclude(tp => tp.Trader)
                                   .Include(t => t.Info)
                                   .ThenInclude(ti => ti.First)
                                   .ThenInclude(tp => tp.TradeOffers)
                                   .Include(t => t.Info)
                                   .ThenInclude(ti => ti.Second)
                                   .ThenInclude(tp => tp.Trader)
                                   .Include(t => t.Info)
                                   .ThenInclude(ti => ti.Second)
                                   .ThenInclude(tp => tp.TradeOffers)
                            .ToArray()).ToModel();
        }

        public async Task<IEnumerable<Trade>> GetAsync()
        => (await _context.Trades.ToArrayAsync()).ToModel();
    }
}
