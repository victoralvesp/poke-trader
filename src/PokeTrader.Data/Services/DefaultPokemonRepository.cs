﻿using PokeTrader.Core.Pokemons.Models;
using PokeTrader.Core.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiNet;
using Pokemon = PokeTrader.Core.Pokemons.Models.Pokemon;

namespace PokeTrader.Data.Services
{
    public class DefaultPokemonRepository : IPokemonRepository
    {
        PokeApiClient _client = new();

        public async Task<IEnumerable<string>> GetPokemonNames(int quantity, int pageOffset = 0)
        {
            var namedApiResourceList = await _client.GetNamedResourcePageAsync<PokeApiNet.Pokemon>(quantity, offset: quantity * pageOffset);
            return from pokeApi in namedApiResourceList.Results
                   select pokeApi.Name;
        }

        public async Task<Pokemon> Get(int id)
        {
            var pokeFromApi = await _client.GetResourceAsync<PokeApiNet.Pokemon>(id);
            return pokeFromApi.Convert();
        }

        public async Task<Pokemon> Get(string name)
        {
            var pokeFromApi = await _client.GetResourceAsync<PokeApiNet.Pokemon>(name);
            return pokeFromApi.Convert();
        }
    }
}
