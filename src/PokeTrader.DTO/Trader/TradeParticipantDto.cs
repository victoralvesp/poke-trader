using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using PokeTrader.Dto.Pokemons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PokeTrader.Dto.Trader
{
    public record TradeParticipantDto : IDto<TradeParticipant>
    {
        [Key]
        public int Id { get; set; }
        public PlayerDto Trader { get; set; }

        public IEnumerable<PokemonDto> TradeOffers { get; set; } = Array.Empty<PokemonDto>();

        public TradeParticipant ToModel()
        => new()
        {
            Trader = Trader.ToModel(),
            TradeOffer = TradeOffers.Select(pk => pk.ToModel()).ToArray()
        };

        public TradeParticipantDto(TradeParticipant model)
        {
            Trader = model.Trader;
            TradeOffers = model.TradeOffer.Select(poke => (PokemonDto)poke).ToArray();
        }
        public TradeParticipantDto()
        { }

        public static implicit operator TradeParticipantDto(TradeParticipant model)
            => new(model);
    }
}