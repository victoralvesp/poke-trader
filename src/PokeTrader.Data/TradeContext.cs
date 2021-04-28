using Microsoft.EntityFrameworkCore;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Pokemons;
using PokeTrader.Dto.Trader;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrader.Data
{
    public class TradeContext : DbContext
    {
        public TradeContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<PlayerDto> Players { get; set; }

        public DbSet<TradeDto> Trades { get; set; }

        public DbSet<TradeInfoDto> TradeInfos { get; set; }

        public DbSet<TradeParticipantDto> TradeParticipants { get; set; }

        public DbSet<PokemonDto> Pokemons { get; set; }
    }
}
