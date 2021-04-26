using System;
using PokeTrader.Core.Resources;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;

namespace PokeTrader.Dto.Trader
{
    public record PlayerDto : IDto<Player>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public PlayerDto(Player model)
        {
            Id = model.Id;
            Name = model.Name;
        }

        public PlayerDto()
        {
        }

        public Player ToModel()
        => new()
        {
            Id = Id,
            Name = Name
        };


        public static implicit operator PlayerDto(Player model)
            => new(model);
    }
}