using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pokemon = PokeTrader.Core.Pokemons.Models.Pokemon;


namespace PokeTrader.Data
{
    public static class ServiceExtensions
    {
        public static Pokemon Convert(this PokeApiNet.Pokemon poke)
            => new()
            {
                Id = poke.Id,
                BaseExperience = poke.BaseExperience,
                Name = poke.Name
            };
    }
}
