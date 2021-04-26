using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using System;
using System.Linq;

namespace PokeTrader.Dto.Trader
{
    public record TradeParticipantDto : IDto<TradeParticipant>
    {
        public PlayerDto Trader { get; set; }

        public int[] TradeOfferIds { get; set; } = Array.Empty<int>();

        public TradeParticipant ToModel()
        => new()
        {
            Trader = Trader.ToModel(),
            TradeOffer = TradeOfferIds.Select(id => BuildUnkownPokemon(id)).ToArray()
        };

        private static Pokemon BuildUnkownPokemon(int id) => new()
        {
            Id = id
        };

        public TradeParticipantDto(TradeParticipant model)
        {
            Trader = model.Trader;
            TradeOfferIds = model.TradeOffer.Select(poke => poke.Id).ToArray();
        }
        public TradeParticipantDto()
        { }

        public static implicit operator TradeParticipantDto(TradeParticipant model)
            => new(model);
    }
}