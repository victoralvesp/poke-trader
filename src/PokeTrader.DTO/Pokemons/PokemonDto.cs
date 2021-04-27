using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Dto.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeTrader.Dto.Pokemons
{
    public record PokemonDto : IDto<Pokemon>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseExperience { get; set; }

        public Pokemon ToModel()
        => new()
        {
            Id = Id,
            Name = Name,
            BaseExperience = BaseExperience
        };

        public PokemonDto()
        {
        }

        public PokemonDto(Pokemon model)
        {
            Id = model.Id;
            Name = model.Name;
            BaseExperience = model.BaseExperience;
        }

        public static implicit operator PokemonDto(Pokemon model)
            => new(model);
    }
}
