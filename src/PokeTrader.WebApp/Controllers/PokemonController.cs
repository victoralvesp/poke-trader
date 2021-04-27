using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeTrader.Core.Pokemons.Manager.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeTrader.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonManager _pokemonManager;

        public PokemonController(IPokemonManager pokemonManager)
        {
            _pokemonManager = pokemonManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int page = 0)
        {
            var pokemons = await _pokemonManager.GetNames(page);
            return Ok(pokemons);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync(string name)
        {
            var pokemon = await _pokemonManager.Get(name);
            return Ok(pokemon);
        }
    }
}
