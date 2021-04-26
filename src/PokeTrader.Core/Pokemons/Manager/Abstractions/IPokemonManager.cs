using System.Collections.Generic;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Pokemons.Models;

namespace PokeTrader.Core.Pokemons.Manager.Abstractions
{
    public interface IPokemonManager
    {
        IEnumerable<Pokemon> Get();
        IEnumerable<Pokemon> Get(IFilter<Pokemon> filter);

        Pokemon? Get(int Id);
    }
}