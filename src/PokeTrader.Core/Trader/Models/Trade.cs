using System;

namespace PokeTrader.Core.Trader.Models
{
    public record Trade
    {
        public int Id { get; init; }
        public TradeInfo Info { get; init; } = new();
        public DateTime TradeDate { get; init; } = DateTime.MaxValue;
    }

}