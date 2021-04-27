using System;
using System.ComponentModel.DataAnnotations;
using PokeTrader.Core.Resources;
using PokeTrader.Core.Trader.Models;
using PokeTrader.Dto.Abstractions;

namespace PokeTrader.Dto.Trader
{
    public record PlayerDto : IDto<Player>
    {
        [Key]
        public string Name { get; set; }



        public PlayerDto(Player model)
        {
            Name = model.Name;
        }

        public PlayerDto()
        {
        }

        public Player ToModel()
        => new()
        {
            Name = Name
        };


        public static implicit operator PlayerDto(Player model)
            => new(model);
    }
}