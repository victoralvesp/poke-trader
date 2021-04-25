using System.ComponentModel.DataAnnotations;

namespace PokeTrader.Core.Pokemons.Models
{
    public class Pokemon
    {
        [Required]
        public int Id { get; init; } = 0;

        public string Name { get; init; } = Resources.TextResources.UnknownPokemon();
    }
}