using System;
using PokeTrader.Core.Resources;

namespace PokeTrader.Core.Trader.Models
{
    public record Player
    {
        public string Name { get; init; } = TextResources.UnknownPlayer();

        public Player(int tradeOrder)
        {
            Name = TextResources.TraderName(tradeOrder);
        }

        public Player()
        {}
    }
}