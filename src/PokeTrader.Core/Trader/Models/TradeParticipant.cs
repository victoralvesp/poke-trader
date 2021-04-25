using PokeTrader.Core.Pokemons.Models;

namespace PokeTrader.Core.Trader.Models
{
    public record TradeParticipant
    {
        public Player Trader { get; init; } = new();

        public Pokemon TradeOffer { get; init; } = new();
    }
}