using System.Collections.Generic;
using System.Threading.Tasks;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Pokemons.Models;

namespace PokeTrader.Core.Pokemons.Manager.Abstractions
{
    public interface IPokemonManager
    {
        Task<IEnumerable<string>> GetNames(int pageOffset = 0);
        Task<Pokemon> Get(string name);

        Task<Pokemon?> Get(int Id);
    }
}