using System;
using System.ComponentModel.DataAnnotations;
using PokeTrader.Core.Pokemons.Models;

namespace PokeTrader.Core.Trader.Models
{
    public record TradeParticipant
    {
        [Required]
        public Player Trader { get; init; } = new();

        [Required]
        [MaxLength(6)]
        [MinLength(1)]
        public Pokemon[] TradeOffer { get; init; } = Array.Empty<Pokemon>();
    }
}