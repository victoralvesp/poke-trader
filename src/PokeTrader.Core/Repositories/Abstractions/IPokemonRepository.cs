using System.Collections.Generic;
using System.Threading.Tasks;
using PokeTrader.Core.Pokemons.Models;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IPokemonRepository
    {
        Task<IEnumerable<Pokemon>> Get();

        Task<Pokemon> Get(int id);
    }
}