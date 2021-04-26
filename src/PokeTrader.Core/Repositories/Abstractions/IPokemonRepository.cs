using System.Collections.Generic;
using System.Threading.Tasks;
using PokeTrader.Core.Pokemons.Models;

namespace PokeTrader.Core.Repositories.Abstractions
{
    public interface IPokemonRepository
    {
        Task<IEnumerable<string>> GetNames(int quantity, int pageOffset = 0);

        Task<Pokemon> Get(int id);

        Task<Pokemon> Get(string name);
    }
}