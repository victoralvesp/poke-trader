using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PokeTrader.Core.Filters.Abstractions;
using PokeTrader.Core.Pokemons.Manager.Abstractions;
using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Repositories.Abstractions;

namespace PokeTrader.Core.Pokemons.Manager
{
    public class DefaultPokemonManager : IPokemonManager
    {
        const int PAGE_SIZE = 50;
        private readonly IPokemonRepository _repo;

        public DefaultPokemonManager(IPokemonRepository repo)
        {
            _repo = repo;
        }

        public async Task<Pokemon?> Get(int id)
        {
            var pokemon = await _repo.Get(id);

            return pokemon;
        }

        public async Task<Pokemon> Get(string name)
        {
            var pokemon = await _repo.Get(name);

            return pokemon;
        }

        public Task<IEnumerable<string>> GetNames(int pageOffset = 0)
        {
            return _repo.GetNames(PAGE_SIZE, pageOffset);
        }
    }
}