using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;
using PokeTrader.Dto.Pokemons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PokeTrader.Dto.Trader
{
    public record TradeParticipantDto : IDto<TradeParticipant>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(PlayerName))]
        public PlayerDto Trader { get; set; }
        public string PlayerName { get; set; }

        [Required]
        [MaxLength(6)]
        [MinLength(1)]
        [ForeignKey(nameof(PokemonDto.TradeParticipationId))]
        public IEnumerable<PokemonDto> TradeOffers { get; set; } = new List<PokemonDto>();

        public TradeParticipant ToModel()
        => new()
        {
            Trader = Trader.ToModel(),
            TradeOffers = TradeOffers.Select(pk => pk.ToModel()).ToArray()
        };

        public TradeParticipantDto(TradeParticipant model)
        {
            Trader = model.Trader;
            TradeOffers = model.TradeOffers.Select(poke => (PokemonDto)poke).ToList();
        }
        public TradeParticipantDto()
        { }

        public static implicit operator TradeParticipantDto(TradeParticipant model)
            => new(model);
    }
}